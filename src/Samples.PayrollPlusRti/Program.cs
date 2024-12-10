// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;
using Payetools.Common.Model;
using Payetools.Hmrc.Common.Rti.Model;
using Payetools.Hmrc.Rti;
using Payetools.Hmrc.Rti.Factories;
using Payetools.Hmrc.Rti.Mapping;
using Payetools.Hmrc.Rti.Model;
using Payetools.Payroll.Hmrc;
using Payetools.Payroll.Model;
using Payetools.Payroll.PayRuns;
using Payetools.ReferenceData.Employer;
using Payetools.Samples.Common;
using Payetools.Samples.Common.Payroll;
using Payetools.Samples.Common.Rti;
using System.Collections.Immutable;

string[] ReferenceDataResources = [@"Resources\HmrcReferenceData_2024_2025.json"];

var vendorId = Environment.GetEnvironmentVariable("HMRC_VENDOR_ID") ??
    throw new InvalidOperationException("Environment variable HMRC_VENDOR_ID must be set in the format user:password");

var creds = Environment.GetEnvironmentVariable("HMRC_CREDENTIALS")?.Split(':', 2) ??
    throw new InvalidOperationException("Environment variable HMRC_CREDENTIALS must be set in the format user:password");

const TestSubmissionMode testMode = TestSubmissionMode.TestInLive;
const PayFrequency payFrequency = PayFrequency.Monthly;

TaxYear taxYear = new TaxYear(TaxYearEnding.Apr5_2025);

// ##### Step 0 - make an Employee #####
var employee = new Employee
{
    Title = "Mr",
    FirstName = "Christopher",
    MiddleNames = "George",
    LastName = "Hampson",
    DateOfBirth = new DateOnly(1961, 7, 15),
    Gender = Gender.Male,
    NiNumber = "NA210196C",
    PostalAddress = new PostalAddress(
        "118 Croxtie Road",
        "Burntwood",
        "Hampshire",
        null,
        "BA14 5WT")
};

// ##### Step 1 - make an Employer #####
var employer = new Employer(
    "ACME WINDOWS AND DOORS LTD",
    "Acme Windows & Doors",
    "120/AB79102",
    "120PF09047612",
    null,  // Corporation tax identifier - not supplied
    false, // Not eligible for Employment Allowance as single director firm
    null,  // State aid qualification for EA - n/a
    true,  // Eligible for Small Employers Relief 
    null); // No bank account supplied

// ##### Step 2 - create an employment for an employee #####
var employment = new Employment
{
    NiCategory = NiCategory.A,
    IsDirector = false,
    NormalHoursWorkedBand = NormalHoursWorkedBand.A,
    PayrollId = "1",
    TaxCode = "1257L",
    StudentLoanInfo = null,
    PensionScheme = null,
};

// ##### Step 3 - create the pay run #####
var payDate = new PayDate(new DateOnly(2024, 5, 20), payFrequency);
var payRunDetails = new PayRunDetails(
    payDate,
    new DateRange(new DateOnly(2024, 5, 1), new DateOnly(2024, 5, 31)));

// ##### Step 4 - create the pay run input #####
var earnings = ImmutableArray.Create<IEarningsEntry>(
    new EarningsEntry
    {
        EarningsDetails = new SalaryEarningsDetails(),
        FixedAmount = 1000.00m
    });
var deductions = ImmutableArray<IDeductionEntry>.Empty;
var payrolledBenefits = ImmutableArray<IPayrolledBenefitForPeriod>.Empty;
var pensionContributions = new PensionContributionLevels();

var payRunInput = new EmployeePayRunInputEntry(
    employment,
    earnings,
    deductions,
    payrolledBenefits,
    pensionContributions);

var payRunEntries = new List<IEmployeePayRunInputEntry>() { payRunInput };

// ##### Step 5 - get a reference data provider, then get a pay run processor and run the pay run #####
var helper = new ReferenceDataHelper(ReferenceDataResources);
var provider = await helper.CreateProviderAsync();
var factory = new PayRunProcessorFactory(provider);

var processor = factory.GetProcessor(payRunDetails);

processor.Process(employer, payRunEntries, out var payRunResult);

foreach (var er in payRunResult.EmployeePayRunResults)
{
    Console.WriteLine($"Employee #{er.Employment.PayrollId}");
    Console.WriteLine($"   Gross pay: {er.TotalGrossPay:c}");
    Console.WriteLine($"   Income tax: {er.TaxCalculationResult.FinalTaxDue:c}");
    Console.WriteLine($"   Employees NI: {er.NiCalculationResult.EmployeeContribution:c} (Employers NI: {er.NiCalculationResult.EmployerContribution:c})");
    Console.WriteLine($"   Student loan repayments: {er.StudentLoanCalculationResult?.TotalDeduction ?? 0.00m:c}");
    Console.WriteLine($"   Employee pension contribution: {er.PensionContributionCalculationResult?.CalculatedEmployeeContributionAmount ?? 0.00m:c}" +
        $" (Employer contribution: {er.PensionContributionCalculationResult?.CalculatedEmployerContributionAmount ?? 0.00m:c})");
    Console.WriteLine($"   Net pay: {er.NetPay:c}");
}

// #### Step 6 - apply the pay run results to the employment history
foreach (var er in payRunResult.EmployeePayRunResults)
{
    er.Employment.UpdatePayrollHistory(payRunEntries[0], er);
}

// #### Step 7 - make an FPS document ####
var logger = Logging.MakeLogger<Program>();

var govTalkMessageFactory = new GovTalkMessageFactory($"{vendorId}", "Vendor-Name", "1.0.0");

var credentials = new RtiCredentials(creds[0], creds[1]);

var documentFactory = new RtiDocumentFactory(taxYear);

var fps = documentFactory.CreateFpsDocument();

var contact = new IRheaderContact(IRheaderContactType.None, new ContactName(["Harry"], "Burton"));

var entries = new List<IFullPaymentSubmissionEmployeeInputEntry>();

foreach (var er in payRunResult.EmployeePayRunResults)
{
    var entry = new FullPaymentSubmissionEmployeeInputEntry(
        employee,
        er,
        null,
        null);

    entries.Add(entry);
}

var fpsInputData = FullPaymentSubmissionDataMapper.Map(
    payRunDetails,
    employer,
    contact,
    IRheaderSenderType.Employer,
    entries,
    null);

fps.Populate(fpsInputData);

var fpsGovTalkMessage = govTalkMessageFactory.CreateMessage(
            RtiDocumentType.FullPaymentSubmission,
            RtiMessageFunction.Submission,
            new TransactionId(),
            credentials,
            testMode,
            employer.HmrcPayeReference ?? throw new InvalidOperationException("PAYE reference cannot be null"),
            "admin@payroll-processing.co.uk",
            fps);

// new Uri("https://test-transaction-engine.tax.service.gov.uk/submission"));

// #### Step 8 - create and submit the FPS ####
var transactionEngineClient = new TransactionEngineClient(
    new SampleHttpClientFactory(),
    new RtiTaskScheduler(),
    new Uri("https://transaction-engine.tax.service.gov.uk/submission"));

using var _ = transactionEngineClient.Subscribe(new SampleTransactionClientMonitor(logger));

await RtiSubmissionHelper.SubmitRtiDocumentAsync(transactionEngineClient, govTalkMessageFactory, fpsGovTalkMessage, logger);

await Task.Delay(30000);

logger.LogInformation("FPS done");

Console.WriteLine("Press any key to continue...");
Console.ReadKey();

// #### Step 9 - create and submit the EPS (should be done between 6th and 19th of the month) 
var employerHistory = new EmployerYtdHistory(taxYear);

payRunResult.GetPayRunSummary(out var payRunSummary);

employerHistory.Apply(payRunSummary);

employerHistory.GetYearToDateFigures(taxYear.GetMonthNumber(payDate.Date, payFrequency), out var ytdHistory);

// NB THIS SHOULD MOVE TO REFERENCE DATA
var reclaimInfo = new StatutoryPaymentReclaimInfo
{
    DefaultReclaimRate = 0.92m,
    SmallEmployersReclaimRate = 1.03m
};

var reclaimCalculator = new StatutoryPaymentReclaimCalculator(reclaimInfo);

reclaimCalculator.Calculate(employer, ytdHistory, out var statutoryPaymentReclaim);

var epsInputData = EmployerPaymentSummaryDataMapper.Map(
    employer,
    new DateOnly(2024, 5, 5),
    contact,
    IRheaderSenderType.Employer,
    statutoryPaymentReclaim,
    null,  // Apprentice Levy
    null); // Final submission data

var eps = documentFactory.CreateEpsDocument();

eps.Populate(epsInputData);

var epsGovTalkMessage = govTalkMessageFactory.CreateMessage(
            RtiDocumentType.EmployerPaymentSummary,
            RtiMessageFunction.Submission,
            new TransactionId(),
            credentials,
            testMode,
            employer.HmrcPayeReference ?? throw new InvalidOperationException("PAYE reference cannot be null"),
            "admin@payroll-processing.co.uk",
            eps);

await RtiSubmissionHelper.SubmitRtiDocumentAsync(transactionEngineClient, govTalkMessageFactory, epsGovTalkMessage, logger);

await Task.Delay(30000);
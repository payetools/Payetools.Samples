﻿// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;
using Payetools.Payroll.Model;
using Payetools.Payroll.PayRuns;
using Payetools.Pensions.Model;
using Payetools.Samples.Common.Payroll;
using PayrollExample;
using System.Collections.Immutable;

string[] ReferenceDataResources = [@"Resources\HmrcReferenceData_2024_2025.json"];

// ##### Step 1 - make an Employer #####
var employer = new Employer(
    "EXAMPLE LTD",
    "Example",
    "121/AB12345",
    "121PF03054321",
    null,  // Corporation tax identifier - not supplied
    false, // Not eligible for Employment Allowance
    null,  // State aid qualification for EA - n/a
    true,  // Eligible for Small Employers Relief 
    null); // No bank account supplied

// ##### Step 2 - create an employment for an employee #####
// (Repeat this step for each employee)
var employment = new Employment
{
    NiCategory = NiCategory.A,
    NormalHoursWorkedBand = NormalHoursWorkedBand.A,
    PayrollId = "1",
    TaxCode = "1257L",
    StudentLoanInfo = new StudentLoanInfo
    {
        StudentLoanType = StudentLoanType.Plan1
    },
    PensionScheme = new PensionScheme
    {
        EarningsBasis = PensionsEarningsBasis.QualifyingEarnings,
        TaxTreatment = PensionTaxTreatment.NetPayArrangement
    }
};

// ##### Step 3 - create the pay run #####
var payDate = new PayDate(new DateOnly(2024, 5, 17), PayFrequency.Monthly);
var payRunDetails = new PayRunDetails(
    payDate,
    new DateRange(new DateOnly(2024, 5, 1), new DateOnly(2024, 5, 31)));

// ##### Step 4 - create the pay run input #####
// (Repeat this step for each employee)
var earnings = ImmutableArray.Create<IEarningsEntry>(
    new EarningsEntry
    {
        EarningsDetails = new SalaryEarningsDetails(),
        FixedAmount = 2500.00m
    });
var deductions = ImmutableArray<IDeductionEntry>.Empty;
var payrolledBenefits = ImmutableArray<IPayrolledBenefitForPeriod>.Empty;
var pensionContributions = new PayrollExample.PensionContributionLevels();

var payRunInput = new EmployeePayRunInputEntry(
    employment,
    earnings,
    deductions,
    payrolledBenefits,
    pensionContributions);

List<IEmployeePayRunInputEntry> payRunEntries = [payRunInput];

// ##### Step 5 - get a reference data provider, then get a pay run processor and run the pay run #####
var helper = new ReferenceDataHelper(ReferenceDataResources);
var provider = await helper.CreateProviderAsync();
var factory = new PayRunProcessorFactory(provider);

var processor = factory.GetProcessor(payRunDetails);

processor.Process(employer, payRunEntries, out var payRunResult);

foreach (var er in payRunResult.EmployeePayRunResults)
{
    Console.WriteLine($"Employee #{er.Employment.PayrollId}:c");
    Console.WriteLine($"   Gross pay: {er.TotalGrossPay:c}");
    Console.WriteLine($"   Income tax: {er.TaxCalculationResult.FinalTaxDue:c}");
    Console.WriteLine($"   Employees NI: {er.NiCalculationResult.EmployeeContribution:c} (Employers NI: {er.NiCalculationResult.EmployerContribution:c})");
    Console.WriteLine($"   Student loan repayments: {er.StudentLoanCalculationResult?.TotalDeduction ?? 0.00m:c}");
    Console.WriteLine($"   Employee pension contribution: {er.PensionContributionCalculationResult?.CalculatedEmployeeContributionAmount ?? 0.00m:c}" +
        $" (Employer contribution: {er.PensionContributionCalculationResult?.CalculatedEmployerContributionAmount ?? 0.00m:c})");
    Console.WriteLine($"   Net pay: {er.NetPay:c}");
}

// #### Step 6 - once finalised, apply the pay run information to the employment history ####
foreach (var er in payRunResult.EmployeePayRunResults)
{
    er.Employment.UpdatePayrollHistory(
        payRunEntries.Where(i => i.Employment.PayrollId == er.Employment.PayrollId).First(),
        er);
}
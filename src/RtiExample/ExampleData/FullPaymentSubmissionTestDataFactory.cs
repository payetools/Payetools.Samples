// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;
using Payetools.Hmrc.Common.Rti.Model;
using Payetools.Hmrc.Rti.Mapping;
using Payetools.Hmrc.Rti.Model;
using Payetools.IncomeTax;
using Payetools.IncomeTax.Model;
using Payetools.NationalInsurance.Model;
using Payetools.NationalInsurance.ReferenceData;
using Payetools.Payroll.Model;
using Payetools.Pensions.Model;
using Payetools.Statutory.AttachmentOfEarnings;
using Payetools.StudentLoans.Model;
using System.Collections.Immutable;

namespace RtiExample.ExampleData;

public static class FullPaymentSubmissionTestDataFactory
{
    public static IFullPaymentSubmissionData MakeFullPaymentSubmissionData(
        IPayRunDetails payRunDetails,
        IRheaderContact contact,
        IRheaderSenderType senderType,
        IEmployer employer,
        IEnumerable<IFpsBodyInputData> entries)
    {
        var employeeEntries = entries.Select(e => MakeEmployeeEntry(e, payRunDetails));

        return FullPaymentSubmissionDataMapper.Map(
            payRunDetails,
            employer,
            contact,
            senderType,
            employeeEntries.Select(e =>
                new FullPaymentSubmissionEmployeeInputEntry(e.Employee, e.PayRunResult, e.NewStarterInfo, e.LateSubmissionReason)),
            null);
    }

    public static (IEmployee Employee, IEmployeePayRunResult PayRunResult, INewStarterInfo? NewStarterInfo, LateSubmissionReason? LateSubmissionReason) MakeEmployeeEntry(
        IFpsBodyInputData data,
        IPayRunDetails payRunDetails)
    {
        var employee = MakeEmployee(data);
        var history = MakeEmployeePayrollHistory(data, payRunDetails.PayDate);
        var employment = MakeEmployment(history, data);
        var payRunResult = MakePayRunResult(payRunDetails, employment, ref history, data);
        var newStarterInfo = MakeNewStarterInfo(employment, data);

        var parsed = Enum.TryParse<LateSubmissionReason>(data.LateReasonCode, out var lateSubmissionReason);

        return (employee, payRunResult, newStarterInfo, parsed ? lateSubmissionReason : null);
    }

    private static IEmployee MakeEmployee(IFpsBodyInputData data) =>
        new TestEmployee(
            data.NiNumber,
            data.DateOfBirth,
            data.Gender switch { "M" => Gender.Male, "F" => Gender.Female, _ => Gender.Unknown },
            null,
            MakePostalAddress(data),
            data.PassportNumber,
            MakeEmployeePartnerDetails(data),
            Title.Parse(data.Title),
            data.FirstName,
            null, //data.Initials.Replace(" ", string.Empty).ToCharArray(),
            data.MiddleName,
            data.LastName,
            null);

    private static PostalAddress MakePostalAddress(IFpsBodyInputData data) =>
        new PostalAddress(
            data.AddressLine1,
            data.AddressLine2,
            data.AddressLine3,
            data.AddressLine4,
            new UkPostcode(data.Postcode),
            null);

    private static IEmployeePartnerDetails? MakeEmployeePartnerDetails(IFpsBodyInputData data) =>
        !string.IsNullOrEmpty(data.PartnerFirstName) && !string.IsNullOrEmpty(data.PartnerLastName) && !string.IsNullOrEmpty(data.PartnerNiNumber) ?
            new EmployeePartnerDetails(
                null,
                data.PartnerFirstName!,
                data.PartnerMiddleName,
                data.PartnerLastName!,
                data.PartnerNiNumber!) : null;

    private static IEmployment MakeEmployment(IEmployeePayrollHistoryYtd history, IFpsBodyInputData data) =>
        new Employment(history)
        {
            PayrollId = data.HasPayrollIdChanged == "yes" ? new PayrollId(data.PayrollId, true, data.OldPayrollId) : new PayrollId(data.PayrollId),
            DirectorsNiCalculationMethod = data.DirectorsNi switch
            {
                "AL" => DirectorsNiCalculationMethod.AlternativeMethod,
                "AN" => DirectorsNiCalculationMethod.StandardAnnualisedEarningsMethod,
                _ => null
            },
            IsDirector = !string.IsNullOrEmpty(data.DirectorsNi),
            DirectorsAppointmentDate = data.DirectorsAppointmentDate,
            IsIrregularlyPaid = data.IsIrregularEmployment == "yes",
            IsOffPayrollWorker = data.IsOffPayrollWorker == "yes",
            EmploymentStartDate = data.StarterStartDate,
            EmploymentEndDate = data.LeavingDate,
            NormalHoursWorkedBand = Enum.Parse<NormalHoursWorkedBand>(data.HoursWorked),
            TaxCode = TaxCode.TryParse(data.TaxCode, out var taxCode) ? taxCode : default!
        };

    private static IEmployeePayRunResult MakePayRunResult(
        IPayRunDetails paymentRunDetails,
        IEmployment employment,
        ref IEmployeePayrollHistoryYtd history,
        IFpsBodyInputData data)
    {
        ITaxCalculationResult taxCalculationResult = new TestTaxCalculationResult(data.TaxDeductedOrRefunded);
        INiCalculationResult niCalculationResult = new TestNiCalculationResult(data, paymentRunDetails.PayDate);
        var hasStudentLoanType = Enum.TryParse<StudentLoanType>(data.StudentLoanType, out var studentLoanType);
        IStudentLoanCalculationResult? studentLoanCalculationResult = new StudentLoanCalculationResult()
        {
            HasPostGradLoan = data.PostgradLoanRecovered != 0,
            StudentLoanDeduction = data.StudentLoanRepayment,
            StudentLoanType = hasStudentLoanType ? studentLoanType : null,
            PostgraduateLoanDeduction = data.PostgradLoanRecovered,
            TotalDeduction = data.StudentLoanRepayment + data.PostgradLoanRecovered
        };
        IPensionContributionCalculationResult? pensionContributionsCalculationResult = new PensionContributionCalculationResult();
        IAttachmentOfEarningsCalculationResult? attachmentOfEarningsCalculationResult = null;

        return new EmployeePayRunResult(
            employment,
            ref taxCalculationResult,
            ref niCalculationResult,
            ref studentLoanCalculationResult,
            ref pensionContributionsCalculationResult,
            ref attachmentOfEarningsCalculationResult,
            data.TaxablePay, // using taxable pay as a proxy for total gross pay
            0.0m, // working gross pay
            data.TaxablePay,
            niCalculationResult.NicablePay,
            data.BenefitsTaxedViaPayroll,
            0.0m, // other deductions
            ref history,
            false, // is leaver in period
            false) // has shared parental pay in period
        {
            IsPaymentAfterLeaving = data.IsPaymentAfterLeaving == "yes"
        };
    }

    // Not currently filling in fields: StatutorySickPayYtd, GrossPayYtd, NicablePayYtd,
    // EmployerPensionContributionsYtd
    private static IEmployeePayrollHistoryYtd MakeEmployeePayrollHistory(IFpsBodyInputData data, PayDate payDate) =>
        new EmployeePayrollHistoryYtd
        {
            EmployeeNiHistoryEntries = MakeNiHistory(data, payDate),
            StatutoryMaternityPayYtd = data.StatutoryMaternityPayYtd,
            StatutoryPaternityPayYtd = data.StatutoryPaternityPayYtd,
            StatutoryAdoptionPayYtd = data.StatutoryAdoptionYtd,
            StatutorySharedParentalPayYtd = data.StatutorySharedPaternityPayYtd,
            StatutoryParentalBereavementPayYtd = data.StatutoryParentalBereavementPayYtd,
            TaxablePayYtd = data.TaxablePayYtd,
            TaxPaidYtd = data.TotalTaxYtd,
            StudentLoanRepaymentsYtd = data.StudentLoansYtd,
            PostgraduateLoanRepaymentsYtd = data.PostgradLoansYtd,
            PayrolledBenefitsYtd = data.BenefitsTaxedViaPayrollYtd,
            EmployeePensionContributionsUnderNpaYtd = data.EmployeePensionContributionsUnderNpaYtd,
            EmployeePensionContributionsUnderRasYtd = data.EmpeePenContribnsNotPaidYtd
        };

    private static NiYtdHistory MakeNiHistory(IFpsBodyInputData data, PayDate payDate)
    {
        IEmployeeNiHistoryEntry[] entries;

        if (GetAllEmployeeNicsInPeriod(data) > 0)
        {
            var entryCount = GetNumberOfNiEntries(data);

            entries = new IEmployeeNiHistoryEntry[entryCount];

            for (int index = 1; index <= entryCount; index++)
                entries[index - 1] = MakeNiYtdHistoryEntry(data, index, payDate);
        }
        else
            entries = Array.Empty<IEmployeeNiHistoryEntry>();

        return new NiYtdHistory(entries.ToImmutableArray(), data.Class1ANICsYtd);
    }

    private static IEmployeeNiHistoryEntry MakeNiYtdHistoryEntry(IFpsBodyInputData data, int index, PayDate payDate) =>
        new TestEmployeeNiHistoryEntry(data, index, payDate);

    private static INewStarterInfo? MakeNewStarterInfo(IEmployment employment, IFpsBodyInputData data) =>
        !string.IsNullOrEmpty(data.StartDeclaration) ?
            new TestNewStarterInfo(data.StartDeclaration, data.StarterContinueStudentLoanPayments == "yes", data.HasPostGradLoanAsStarter == "yes") :
            null;

    private class TestTaxCalculationResult : ITaxCalculationResult
    {
        public ITaxCalculator Calculator => throw new NotImplementedException();
        public TaxCode TaxCode => throw new NotImplementedException();
        public decimal TaxableSalaryAfterTaxFreePay => throw new NotImplementedException();
        public decimal TaxFreePayToEndOfPeriod => throw new NotImplementedException();
        public decimal PreviousPeriodSalaryYearToDate => throw new NotImplementedException();
        public decimal PreviousPeriodTaxPaidYearToDate => throw new NotImplementedException();
        public decimal TaxUnpaidDueToRegulatoryLimit => throw new NotImplementedException();
        public decimal FinalTaxDue { get; }
        public decimal TaxDueBeforeApplicationOfRegulatoryLimit => throw new NotImplementedException();
        public int HighestApplicableTaxBandIndex => throw new NotImplementedException();
        public decimal IncomeAtHighestApplicableBand => throw new NotImplementedException();
        public decimal TaxAtHighestApplicableBand => throw new NotImplementedException();

        public TestTaxCalculationResult(decimal finalTaxDue)
        {
            FinalTaxDue = finalTaxDue;
        }
    }

    private class TestNiCalculationResult : INiCalculationResult
    {
        public NiCategory NiCategory { get; }
        public decimal NicablePay { get; }
        public INiCategoryRatesEntry RatesUsed => throw new NotImplementedException();
        public INiThresholdSet ThresholdsUsed => throw new NotImplementedException();
        public NiEarningsBreakdown EarningsBreakdown { get; }
        public decimal EmployeeContribution { get; }
        public decimal EmployerContribution { get; }
        public decimal TotalContribution => throw new NotImplementedException();
        public bool NoRecordingRequired => false;
        public decimal? Class1ANicsPayable { get; }

        public TestNiCalculationResult(IFpsBodyInputData data, PayDate payDate)
        {
            if (GetAllEmployeeNicsInPeriod(data) == 0)
                return;

            int index;

            // Find the index for the column that has non-zero employee contributions - that's our "current" period NI
            for (index = 1; index <= 4; index++)
                if (GetNiTestValue(data, "EmployeeContribnsInPd", index) != 0)
                    break;

            NiCategory = GetNiTestCategory(data, index, payDate);
            NicablePay = GetNiTestValue(data, "GrossEarningsForNicsInPeriod", index);
            EmployeeContribution = GetNiTestValue(data, "EmployeeContribnsInPd", index);
            EmployerContribution = GetNiTestValue(data, "TotalEmpNICInPeriod", index);

            EarningsBreakdown = new NiEarningsBreakdown();
        }
    }

    public class TestEmployeeNiHistoryEntry : IEmployeeNiHistoryEntry
    {
        public NiCategory NiCategoryPertaining { get; }

        public decimal GrossNicableEarnings { get; }

        public decimal EmployeeContribution { get; }

        public decimal EmployerContribution { get; }

        public decimal TotalContribution { get; }

        public decimal EarningsAtLEL { get; }

        public decimal EarningsAboveLELUpToAndIncludingST { get; }

        public decimal EarningsAboveSTUpToAndIncludingPT { get; }

        public decimal EarningsAbovePTUpToAndIncludingFUST { get; }

        public decimal EarningsAboveFUSTUpToAndIncludingUEL { get; }

        public decimal EarningsAboveUEL { get; }

        public decimal EarningsAboveSTUpToAndIncludingUEL { get; }

        public IEmployeeNiHistoryEntry Add(in INiCalculationResult result) =>
            throw new NotImplementedException();

        public TestEmployeeNiHistoryEntry(IFpsBodyInputData data, int index, PayDate payDate)
        {
            NiCategoryPertaining = GetNiTestCategory(data, index, payDate);
            GrossNicableEarnings = GetNiTestValue(data, "GrossEarningsForNicsYtd", index);
            EmployeeContribution = GetNiTestValue(data, "EmployeeContribnsYtd", index);
            EmployerContribution = GetNiTestValue(data, "TotalEmpNICYtd", index);
            TotalContribution = 0.0m;                   // Not used
            EarningsAtLEL = GetNiTestValue(data, "AtLELYtd", index);
            EarningsAboveLELUpToAndIncludingST = 0.0m;  // Obvious hack as FPS doesn't split LEL to ST and ST to PT
            EarningsAboveSTUpToAndIncludingPT = GetNiTestValue(data, "LELtoPTYtd", index);
            EarningsAbovePTUpToAndIncludingFUST = 0.0m; // Obvious hack as FPS doesn't split PT to FUST and FUST to UEL
            EarningsAboveFUSTUpToAndIncludingUEL = GetNiTestValue(data, "PTtoUELYtd", index);
            EarningsAboveUEL = 0.0m;                    // Not used
            EarningsAboveSTUpToAndIncludingUEL = 0.0m;  // Not used
        }
    }

    public static int GetNumberOfNiEntries(IFpsBodyInputData data)
    {
        int count = 0;

        if (!string.IsNullOrEmpty(data.NiCategory_1))
            count++;

        if (!string.IsNullOrEmpty(data.NiCategory_2))
            count++;

        if (!string.IsNullOrEmpty(data.NiCategory_3))
            count++;

        if (!string.IsNullOrEmpty(data.NiCategory_4))
            count++;

        return count;
    }

    private class TestNewStarterInfo : INewStarterInfo
    {
        public StarterDeclaration? StarterDeclaration { get; }
        public bool StudentLoanDeductionNeeded { get; }
        public StudentLoanType? StudentLoanType { get; }
        public bool PostgraduateLoanDeductionNeeded { get; }

        public TestNewStarterInfo(
            string starterDeclaration,
            bool studentLoanDeductionNeeded,
            bool graduateLoanDeductionNeeded)
        {
            var parsed = Enum.TryParse<StarterDeclaration>(starterDeclaration, out var declaration);

            StarterDeclaration = parsed ? declaration : throw new InvalidOperationException($"Unrecognised starter declaration '{starterDeclaration}'");
            StudentLoanDeductionNeeded = studentLoanDeductionNeeded;
            StudentLoanType = null;
            PostgraduateLoanDeductionNeeded = graduateLoanDeductionNeeded;
        }
    }

    private static decimal GetNiTestValue(IFpsBodyInputData data, string name, int index) =>
        name switch
        {
            "AtLELYtd" => index switch { 1 => data.AtLELYtd_1, 2 => data.AtLELYtd_2, 3 => data.AtLELYtd_3, 4 => data.AtLELYtd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "EmployeeContribnsInPd" => index switch { 1 => data.EmployeeContribnsInPd_1, 2 => data.EmployeeContribnsInPd_2, 3 => data.EmployeeContribnsInPd_3, 4 => data.EmployeeContribnsInPd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "EmployeeContribnsYtd" => index switch { 1 => data.EmployeeContribnsYtd_1, 2 => data.EmployeeContribnsYtd_2, 3 => data.EmployeeContribnsYtd_3, 4 => data.EmployeeContribnsYtd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "GrossEarningsForNicsInPeriod" => index switch { 1 => data.GrossEarningsForNicsInPeriod_1, 2 => data.GrossEarningsForNicsInPeriod_2, 3 => data.GrossEarningsForNicsInPeriod_3, 4 => data.GrossEarningsForNicsInPeriod_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "GrossEarningsForNicsYtd" => index switch { 1 => data.GrossEarningsForNicsYtd_1, 2 => data.GrossEarningsForNicsYtd_2, 3 => data.GrossEarningsForNicsYtd_3, 4 => data.GrossEarningsForNicsYtd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "LELtoPTYtd" => index switch { 1 => data.LELtoPTYtd_1, 2 => data.LELtoPTYtd_2, 3 => data.LELtoPTYtd_3, 4 => data.LELtoPTYtd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "PTtoUELYtd" => index switch { 1 => data.PTtoUELYtd_1, 2 => data.PTtoUELYtd_2, 3 => data.PTtoUELYtd_3, 4 => data.PTtoUELYtd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "TotalEmpNICInPeriod" => index switch { 1 => data.TotalEmpNICInPeriod_1, 2 => data.TotalEmpNICInPeriod_2, 3 => data.TotalEmpNICInPeriod_3, 4 => data.TotalEmpNICInPeriod_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            "TotalEmpNICYtd" => index switch { 1 => data.TotalEmpNICYtd_1, 2 => data.TotalEmpNICYtd_2, 3 => data.TotalEmpNICYtd_3, 4 => data.TotalEmpNICYtd_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) },
            _ => throw new ArgumentException($"Unrecognised value for name '{name}'", nameof(name))
        };

    private static NiCategory GetNiTestCategory(IFpsBodyInputData data, int index, PayDate payDate) =>
        (index switch { 1 => data.NiCategory_1, 2 => data.NiCategory_2, 3 => data.NiCategory_3, 4 => data.NiCategory_4, _ => throw new ArgumentException("Index must be between 1 and 4", nameof(index)) }).ToNiCategory(payDate.TaxYear.TaxYearEnding) ??
            throw new ArgumentException("NiCategory_ values cannot be null", nameof(data));

    private static decimal GetAllEmployeeNicsInPeriod(IFpsBodyInputData data) =>
        data.EmployeeContribnsInPd_1 + data.EmployeeContribnsInPd_2 + data.EmployeeContribnsInPd_3 + data.EmployeeContribnsInPd_4;
}

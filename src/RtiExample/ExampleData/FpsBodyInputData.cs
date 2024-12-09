// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;

namespace RtiExample.ExampleData;

public class FpsBodyInputData : IFpsBodyInputData
{
    public string DefinitionVersion { get; set; } = string.Empty;
    public string TestIdentifier { get; set; } = string.Empty;
    public TaxYearEnding TaxYearEnding { get; set; }
    public string RelatesTo { get; set; } = string.Empty;
    public string AccountsOfficeRef { get; set; } = string.Empty;
    public string? CompanyTaxRef { get; set; }
    public string PayeOfficeNumber { get; set; } = string.Empty;
    public string PayeReference { get; set; } = string.Empty;
    public string RelatedTaxYear { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string AddressLine3 { get; set; } = string.Empty;
    public string AddressLine4 { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string NiNumber { get; set; } = string.Empty;
    public string PartnerFirstName { get; set; } = string.Empty;
    public string PartnerMiddleName { get; set; } = string.Empty;
    public string PartnerLastName { get; set; } = string.Empty;
    public string PartnerNiNumber { get; set; } = string.Empty;
    public string? PassportNumber { get; set; }
    public string? DirectorsNi { get; set; }
    public decimal BenefitsTaxedViaPayrollYtd { get; set; }
    public decimal EmpeePenContribnsNotPaidYtd { get; set; }
    public decimal EmployeePensionContributionsUnderNpaYtd { get; set; }
    public decimal PostgradLoansYtd { get; set; }
    public decimal StudentLoansYtd { get; set; }
    public decimal TaxablePayYtd { get; set; }
    public decimal TotalTaxYtd { get; set; }
    public string? IsIrregularEmployment { get; set; }
    public DateOnly? LeavingDate { get; set; }
    public string? IsOffPayrollWorker { get; set; }
    public string PayrollId { get; set; } = string.Empty;
    public string? OldPayrollId { get; set; }
    public string? HasPayrollIdChanged { get; set; }
    public string? BacsHashCode { get; set; }
    public decimal BenefitsTaxedViaPayroll { get; set; }
    public decimal Class1ANICsYtd { get; set; }
    public decimal DeductionsFromNetPay { get; set; }
    public decimal EmployeePensionContribsNotUnderNpa { get; set; }
    public decimal EmployeePensionContribsUnderNpa { get; set; }
    public string HoursWorked { get; set; } = string.Empty;
    public string? LateReasonCode { get; set; }
    public decimal PayAfterStatDeductions { get; set; }
    public string PayFreq { get; set; } = string.Empty;
    public int PeriodsCovered { get; set; }
    public string? IsPaymentAfterLeaving { get; set; }
    public DateOnly PmtDate { get; set; }
    public decimal PostgradLoanRecovered { get; set; }
    public decimal StatutoryAdoptionYtd { get; set; }
    public decimal StatutorySharedPaternityPayYtd { get; set; }
    public decimal StatutoryMaternityPayYtd { get; set; }
    public decimal StatutoryParentalBereavementPayYtd { get; set; }
    public decimal StatutoryPaternityPayYtd { get; set; }
    public decimal StudentLoanRepayment { get; set; }
    public string? StudentLoanType { get; set; }
    public decimal TaxablePay { get; set; }
    public string TaxCode { get; set; } = string.Empty;
    public string? NonCumulativeTaxCode { get; set; }
    public string? TaxRegime { get; set; }
    public decimal TaxDeductedOrRefunded { get; set; }
    public bool UnpaidAbsence { get; set; }
    public string? HasPostGradLoanAsStarter { get; set; }
    public DateOnly StarterStartDate { get; set; }
    public string? StartDeclaration { get; set; }
    public string? StarterContinueStudentLoanPayments { get; set; }
    public DateOnly? DirectorsAppointmentDate { get; set; }
    public decimal AtLELYtd_1 { get; set; }
    public decimal EmployeeContribnsInPd_1 { get; set; }
    public decimal EmployeeContribnsYtd_1 { get; set; }
    public decimal GrossEarningsForNicsInPeriod_1 { get; set; }
    public decimal GrossEarningsForNicsYtd_1 { get; set; }
    public decimal LELtoPTYtd_1 { get; set; }
    public string NiCategory_1 { get; set; } = string.Empty;
    public decimal PTtoUELYtd_1 { get; set; }
    public decimal TotalEmpNICInPeriod_1 { get; set; }
    public decimal TotalEmpNICYtd_1 { get; set; }
    public decimal AtLELYtd_2 { get; set; }
    public decimal EmployeeContribnsInPd_2 { get; set; }
    public decimal EmployeeContribnsYtd_2 { get; set; }
    public decimal GrossEarningsForNicsInPeriod_2 { get; set; }
    public decimal GrossEarningsForNicsYtd_2 { get; set; }
    public decimal LELtoPTYtd_2 { get; set; }
    public string NiCategory_2 { get; set; } = string.Empty;
    public decimal PTtoUELYtd_2 { get; set; }
    public decimal TotalEmpNICInPeriod_2 { get; set; }
    public decimal TotalEmpNICYtd_2 { get; set; }
    public decimal AtLELYtd_3 { get; set; }
    public decimal EmployeeContribnsInPd_3 { get; set; }
    public decimal EmployeeContribnsYtd_3 { get; set; }
    public decimal GrossEarningsForNicsInPeriod_3 { get; set; }
    public decimal GrossEarningsForNicsYtd_3 { get; set; }
    public decimal LELtoPTYtd_3 { get; set; }
    public string NiCategory_3 { get; set; } = string.Empty;
    public decimal PTtoUELYtd_3 { get; set; }
    public decimal TotalEmpNICInPeriod_3 { get; set; }
    public decimal TotalEmpNICYtd_3 { get; set; }
    public decimal AtLELYtd_4 { get; set; }
    public decimal EmployeeContribnsInPd_4 { get; set; }
    public decimal EmployeeContribnsYtd_4 { get; set; }
    public decimal GrossEarningsForNicsInPeriod_4 { get; set; }
    public decimal GrossEarningsForNicsYtd_4 { get; set; }
    public decimal LELtoPTYtd_4 { get; set; }
    public string NiCategory_4 { get; set; } = string.Empty;
    public decimal PTtoUELYtd_4 { get; set; }
    public decimal TotalEmpNICInPeriod_4 { get; set; }
    public decimal TotalEmpNICYtd_4 { get; set; }
}

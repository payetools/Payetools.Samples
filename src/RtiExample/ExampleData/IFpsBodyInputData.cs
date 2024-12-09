// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;

namespace RtiExample.ExampleData;

public interface IFpsBodyInputData
{
    string DefinitionVersion { get; }
    string TestIdentifier { get; }
    TaxYearEnding TaxYearEnding { get; }
    string RelatesTo { get; }
    string AccountsOfficeRef { get; }
    string? CompanyTaxRef { get; }
    string PayeOfficeNumber { get; }
    string PayeReference { get; }
    string RelatedTaxYear { get; }
    string Postcode { get; }
    string AddressLine1 { get; }
    string AddressLine2 { get; }
    string AddressLine3 { get; }
    string AddressLine4 { get; }
    DateOnly DateOfBirth { get; }
    string Gender { get; }
    string FirstName { get; }
    string MiddleName { get; }
    string LastName { get; }
    string Title { get; }
    string NiNumber { get; }
    string PartnerFirstName { get; }
    string PartnerMiddleName { get; }
    string PartnerLastName { get; }
    string PartnerNiNumber { get; }
    string? PassportNumber { get; }
    string? DirectorsNi { get; }
    decimal BenefitsTaxedViaPayrollYtd { get; }
    decimal EmpeePenContribnsNotPaidYtd { get; }
    decimal EmployeePensionContributionsUnderNpaYtd { get; }
    decimal PostgradLoansYtd { get; }
    decimal StudentLoansYtd { get; }
    decimal TaxablePayYtd { get; }
    decimal TotalTaxYtd { get; }
    string? IsIrregularEmployment { get; }
    DateOnly? LeavingDate { get; }
    string? IsOffPayrollWorker { get; }
    string PayrollId { get; }
    string? OldPayrollId { get; }
    string? HasPayrollIdChanged { get; }
    string? BacsHashCode { get; }
    decimal BenefitsTaxedViaPayroll { get; }
    decimal Class1ANICsYtd { get; }
    decimal DeductionsFromNetPay { get; }
    decimal EmployeePensionContribsNotUnderNpa { get; }
    decimal EmployeePensionContribsUnderNpa { get; }
    string HoursWorked { get; }
    string? LateReasonCode { get; }
    decimal PayAfterStatDeductions { get; }
    string PayFreq { get; }
    int PeriodsCovered { get; }
    string? IsPaymentAfterLeaving { get; }
    DateOnly PmtDate { get; }
    decimal PostgradLoanRecovered { get; }
    decimal StatutoryAdoptionYtd { get; }
    decimal StatutorySharedPaternityPayYtd { get; }
    decimal StatutoryMaternityPayYtd { get; }
    decimal StatutoryParentalBereavementPayYtd { get; }
    decimal StatutoryPaternityPayYtd { get; }
    decimal StudentLoanRepayment { get; }
    string? StudentLoanType { get; }
    decimal TaxablePay { get; }
    string TaxCode { get; }
    string? NonCumulativeTaxCode { get; }
    string? TaxRegime { get; }
    decimal TaxDeductedOrRefunded { get; }
    bool UnpaidAbsence { get; }
    string? HasPostGradLoanAsStarter { get; }
    DateOnly StarterStartDate { get; }
    string? StartDeclaration { get; }
    string? StarterContinueStudentLoanPayments { get; }
    DateOnly? DirectorsAppointmentDate { get; }
    decimal AtLELYtd_1 { get; }
    decimal EmployeeContribnsInPd_1 { get; }
    decimal EmployeeContribnsYtd_1 { get; }
    decimal GrossEarningsForNicsInPeriod_1 { get; }
    decimal GrossEarningsForNicsYtd_1 { get; }
    decimal LELtoPTYtd_1 { get; }
    string NiCategory_1 { get; }
    decimal PTtoUELYtd_1 { get; }
    decimal TotalEmpNICInPeriod_1 { get; }
    decimal TotalEmpNICYtd_1 { get; }
    decimal AtLELYtd_2 { get; }
    decimal EmployeeContribnsInPd_2 { get; }
    decimal EmployeeContribnsYtd_2 { get; }
    decimal GrossEarningsForNicsInPeriod_2 { get; }
    decimal GrossEarningsForNicsYtd_2 { get; }
    decimal LELtoPTYtd_2 { get; }
    string NiCategory_2 { get; }
    decimal PTtoUELYtd_2 { get; }
    decimal TotalEmpNICInPeriod_2 { get; }
    decimal TotalEmpNICYtd_2 { get; }
    decimal AtLELYtd_3 { get; }
    decimal EmployeeContribnsInPd_3 { get; }
    decimal EmployeeContribnsYtd_3 { get; }
    decimal GrossEarningsForNicsInPeriod_3 { get; }
    decimal GrossEarningsForNicsYtd_3 { get; }
    decimal LELtoPTYtd_3 { get; }
    string NiCategory_3 { get; }
    decimal PTtoUELYtd_3 { get; }
    decimal TotalEmpNICInPeriod_3 { get; }
    decimal TotalEmpNICYtd_3 { get; }
    decimal AtLELYtd_4 { get; }
    decimal EmployeeContribnsInPd_4 { get; }
    decimal EmployeeContribnsYtd_4 { get; }
    decimal GrossEarningsForNicsInPeriod_4 { get; }
    decimal GrossEarningsForNicsYtd_4 { get; }
    decimal LELtoPTYtd_4 { get; }
    string NiCategory_4 { get; }
    decimal PTtoUELYtd_4 { get; }
    decimal TotalEmpNICInPeriod_4 { get; }
    decimal TotalEmpNICYtd_4 { get; }
}

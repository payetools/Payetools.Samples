// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;
using Payetools.Payroll.Model;

namespace RtiExample.ExampleData;

public class TestEmployer : IEmployer
{
    public class TestBankAccount : IBankAccount
    {
        public string AccountName { get; }

        public string AccountNumber { get; }

        public string SortCode { get; }

        public string? BuildingSocietyReference { get; }

        public TestBankAccount(string? accountName, string? accountNumber, string? sortCode, string? buildingSocietyReference = null)
        {
            AccountName = accountName ?? string.Empty;
            AccountNumber = accountNumber ?? string.Empty;
            SortCode = sortCode ?? string.Empty;
            BuildingSocietyReference = buildingSocietyReference;
        }
    }

    public string? OfficialName => throw new NotImplementedException();

    public string KnownAsName => throw new NotImplementedException();

    public HmrcPayeReference? HmrcPayeReference { get; }

    public HmrcAccountsOfficeReference? AccountsOfficeReference { get; }

    public string? HmrcCorporationTaxReference { get; }

    public bool IsEligibleForSmallEmployersRelief => true;

    public bool IsEligibleForEmploymentAllowance { get; set; }

    public StateAidForEmploymentAllowance? EmploymentAllowanceStateAidClassification { get; set; }

    public bool IsApprenticeLevyDue => false;

    public decimal? ApprenticeLevyAllowance => null;

    public IBankAccount? BankAccount { get; }

    public TestEmployer(
        HmrcPayeReference? hmrcPayeReference,
        HmrcAccountsOfficeReference? accountsOfficeReference,
        string? corporationTaxReference,
        IBankAccount? bankAccount = null)
    {
        HmrcPayeReference = hmrcPayeReference;
        AccountsOfficeReference = accountsOfficeReference;
        HmrcCorporationTaxReference = corporationTaxReference;
        BankAccount = bankAccount;
    }
}
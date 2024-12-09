// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Pensions.Model;

namespace PayrollExample;

internal class PensionContributionLevels : IPensionContributionLevels
{
    public decimal EmployeeContribution => 5.0m;

    public bool EmployeeContributionIsFixedAmount => false;

    public decimal EmployerContribution => 3.0m;

    public bool EmployerContributionIsFixedAmount => false;

    public bool SalaryExchangeApplied => false;

    public decimal? EmployersNiReinvestmentPercentage => null;

    public decimal? AvcForPeriod => null;

    public decimal? SalaryForMaternityPurposes => null;
}
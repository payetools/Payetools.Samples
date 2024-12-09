// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Payroll.Model;

namespace PayrollExample;

internal class SalaryEarningsDetails : IEarningsDetails
{
    public string Name => "Salary";

    public PaymentType PaymentType => PaymentType.GeneralEarnings;

    public PayRateUnits? Units => PayRateUnits.PerPayPeriod;

    public bool IsSubjectToTax => true;

    public bool IsSubjectToNi => true;

    public bool IsPensionable => true;

    public bool IsNetToGross => false;

    public bool IsTreatedAsOvertime => false;
}
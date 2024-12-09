// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Extensions;
using Payetools.Common.Model;
using Payetools.Hmrc.Common.Rti.Model;
using Payetools.Hmrc.Rti.Extensions;
using Payetools.Hmrc.Rti.Mapping;
using Payetools.Payroll.Model;
using RtiExample.ExampleData;

namespace RtiExample.ExampleData;

public static class EmployerPaymentSummaryTestDataFactory
{
    public static IEmployerPaymentSummaryData MakeEmployerPaymentSummaryData(
        IPayRunDetails payRunDetails,
        IRheaderContact irheaderContact,
        IRheaderSenderType senderType,
        IEmployer employer,
        IEpsInputData data)
    {
        var noPaymentsForPeriod = MakeDateRange(data.NoPaymentDates_From, data.NoPaymentDates_To);
        var periodOfInactivity = MakeDateRange(data.PeriodOfInactivity_From, data.PeriodOfInactivity_To);

        var statutoryPaymentReclaim = MakeRecoverableAmountsYtd(data);

        var epsData = statutoryPaymentReclaim != null ?
            EmployerPaymentSummaryDataMapper.Map(
                employer,
                payRunDetails.PayPeriod.End,
                irheaderContact,
                senderType,
                statutoryPaymentReclaim,
                MakeApprenticeLevyInfo(data),
                MakeFinalSubmission(data)) :
            EmployerPaymentSummaryDataMapper.Map(
                employer,
                payRunDetails.PayPeriod.End,
                irheaderContact,
                senderType,
                noPaymentsForPeriod ?? periodOfInactivity,
                periodOfInactivity != null,
                MakeApprenticeLevyInfo(data),
                MakeFinalSubmission(data));

        return epsData;
    }

    private static DateRange? MakeDateRange(DateOnly? datesFrom, DateOnly? datesTo) =>
        (datesFrom, datesTo) switch
        {
            (null, null) => null!,
            (DateOnly from, DateOnly to) => new DateRange(from, to),
            _ => throw new ArgumentException("If NoPaymentsData supplied, both From and To are required", nameof(datesFrom))
        };

    private static IStatutoryPaymentReclaim? MakeRecoverableAmountsYtd(IEpsInputData data)
    {
        if (data.RAYTD_TaxMonth == null &&
            data.RAYTD_SMPRecovered == null &&
            data.RAYTD_SPPRecovered == null &&
            data.RAYTD_SAPRecovered == null &&
            data.RAYTD_ShPPRecovered == null &&
            data.RAYTD_SPBPRecovered == null &&
            data.RAYTD_NICCompensationOnSMP == null &&
            data.RAYTD_NICCompensationOnSPP == null &&
            data.RAYTD_NICCompensationOnSAP == null &&
            data.RAYTD_NICCompensationOnShPP == null &&
            data.RAYTD_NICCompensationOnSPBP == null &&
            data.RAYTD_CISDeductionsSuffered == null)
        {
            return null;
        }
        else
        {
            return new StatutoryPaymentReclaim
            {
                ReclaimableStatutoryMaternityPay = (data.RAYTD_SMPRecovered ?? 0.0m).To2DPs(),
                ReclaimableStatutoryPaternityPay = (data.RAYTD_SPPRecovered ?? 0.0m).To2DPs(),
                ReclaimableStatutoryAdoptionPay = (data.RAYTD_SAPRecovered ?? 0.0m).To2DPs(),
                ReclaimableStatutorySharedParentalPay = (data.RAYTD_ShPPRecovered ?? 0.0m).To2DPs(),
                ReclaimableStatutoryParentalBereavementPay = (data.RAYTD_SPBPRecovered ?? 0.0m).To2DPs(),
                AdditionalNiCompensationOnSMP = (data.RAYTD_NICCompensationOnSMP ?? 0.0m).To2DPs(),
                AdditionalNiCompensationOnSPP = (data.RAYTD_NICCompensationOnSPP ?? 0.0m).To2DPs(),
                AdditionalNiCompensationOnSAP = (data.RAYTD_NICCompensationOnSAP ?? 0.0m).To2DPs(),
                AdditionalNiCompensationOnSShPP = (data.RAYTD_NICCompensationOnShPP ?? 0.0m).To2DPs(),
                AdditionalNiCompensationOnSPBP = (data.RAYTD_NICCompensationOnSPBP ?? 0.0m).To2DPs(),
                CisDeductionsSuffered = (data.RAYTD_CISDeductionsSuffered ?? 0.0m).To2DPs()
            };
        }
    }

    private static IApprenticeLevy? MakeApprenticeLevyInfo(IEpsInputData data)
    {
        if (data.ApprenticeLevy_AnnualAllce == null || data.ApprenticeLevy_LevyDueYTD == null || data.ApprenticeLevy_TaxMonth == null)
            return null;

        return new ApprenticeLevy((decimal)data.ApprenticeLevy_LevyDueYTD, data.ApprenticeLevy_TaxMonth, (decimal)data.ApprenticeLevy_AnnualAllce);
    }

    private static IFinalSubmissionData? MakeFinalSubmission(IEpsInputData data)
    {
        var schemeCeased = data.FinalSubmission_BecauseSchemeCeased;
        var finalForYear = data.FinalSubmission_ForYear;

        if (schemeCeased == null && finalForYear == null)
            return null;

        return new FinalSubmissionData
        {
            FinalSubmissionType = schemeCeased == "yes" ? FinalSubmissionType.SchemeCeasing :
                finalForYear == "yes" ? FinalSubmissionType.FinalFpsForTaxYear :
                    throw new ArgumentException("Either scheme ceasing or final for year must be specified", nameof(data)),
            DateSchemeCeased = data.FinalSubmission_DateSchemeCeased is DateOnly d ? d.ToDateTimeUnspecified() : null
        };
    }

    private class ApprenticeLevy : IApprenticeLevy
    {
        public decimal LevyDueYtd { get; init; }

        public string TaxMonth { get; init; }

        public decimal AnnualAllowance { get; init; }

        public ApprenticeLevy(decimal levyDueYtd, string taxMonth, decimal annualAllowance)
        {
            LevyDueYtd = levyDueYtd;
            TaxMonth = taxMonth;
            AnnualAllowance = annualAllowance;
        }
    }
}
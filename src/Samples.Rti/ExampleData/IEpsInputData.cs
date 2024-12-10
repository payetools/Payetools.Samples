// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;

namespace RtiExample.ExampleData;

public interface IEpsInputData
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
    string? NoPaymentForPeriod { get; }
    DateOnly? NoPaymentDates_From { get; }
    DateOnly? NoPaymentDates_To { get; }
    DateOnly? PeriodOfInactivity_From { get; }
    DateOnly? PeriodOfInactivity_To { get; }
    string? EmpAllceInd { get; }
    string? DeMinimisStateAid_Agri { get; }
    string? DeMinimisStateAid_FisheriesAqua { get; }
    string? DeMinimisStateAid_RoadTrans { get; }
    string? DeMinimisStateAid_Indust { get; }
    string? DeMinimisStateAid_NA { get; }
    string? RAYTD_TaxMonth { get; }
    decimal? RAYTD_SMPRecovered { get; }
    decimal? RAYTD_SPPRecovered { get; }
    decimal? RAYTD_SAPRecovered { get; }
    decimal? RAYTD_ShPPRecovered { get; }
    decimal? RAYTD_SPBPRecovered { get; }
    decimal? RAYTD_NICCompensationOnSMP { get; }
    decimal? RAYTD_NICCompensationOnSPP { get; }
    decimal? RAYTD_NICCompensationOnSAP { get; }
    decimal? RAYTD_NICCompensationOnShPP { get; }
    decimal? RAYTD_NICCompensationOnSPBP { get; }
    decimal? RAYTD_CISDeductionsSuffered { get; }
    decimal? ApprenticeLevy_LevyDueYTD { get; }
    string? ApprenticeLevy_TaxMonth { get; }
    decimal? ApprenticeLevy_AnnualAllce { get; }
    string? Account_AccountHoldersName { get; }
    string? Account_AccountNo { get; }
    string? Account_SortCode { get; }
    string? Account_BuildingSocRef { get; }
    string? FinalSubmission_BecauseSchemeCeased { get; }
    DateOnly? FinalSubmission_DateSchemeCeased { get; }
    string? FinalSubmission_ForYear { get; }
}

// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;

namespace RtiExample.ExampleData;

public class EpsInputData : IEpsInputData
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
    public string? NoPaymentForPeriod { get; set; }
    public DateOnly? NoPaymentDates_From { get; set; }
    public DateOnly? NoPaymentDates_To { get; set; }
    public DateOnly? PeriodOfInactivity_From { get; set; }
    public DateOnly? PeriodOfInactivity_To { get; set; }
    public string? EmpAllceInd { get; set; }
    public string? DeMinimisStateAid_Agri { get; set; }
    public string? DeMinimisStateAid_FisheriesAqua { get; set; }
    public string? DeMinimisStateAid_RoadTrans { get; set; }
    public string? DeMinimisStateAid_Indust { get; set; }
    public string? DeMinimisStateAid_NA { get; set; }
    public string? RAYTD_TaxMonth { get; set; }
    public decimal? RAYTD_SMPRecovered { get; set; }
    public decimal? RAYTD_SPPRecovered { get; set; }
    public decimal? RAYTD_SAPRecovered { get; set; }
    public decimal? RAYTD_ShPPRecovered { get; set; }
    public decimal? RAYTD_SPBPRecovered { get; set; }
    public decimal? RAYTD_NICCompensationOnSMP { get; set; }
    public decimal? RAYTD_NICCompensationOnSPP { get; set; }
    public decimal? RAYTD_NICCompensationOnSAP { get; set; }
    public decimal? RAYTD_NICCompensationOnShPP { get; set; }
    public decimal? RAYTD_NICCompensationOnSPBP { get; set; }
    public decimal? RAYTD_CISDeductionsSuffered { get; set; }
    public decimal? ApprenticeLevy_LevyDueYTD { get; set; }
    public string? ApprenticeLevy_TaxMonth { get; set; }
    public decimal? ApprenticeLevy_AnnualAllce { get; set; }
    public string? Account_AccountHoldersName { get; set; }
    public string? Account_AccountNo { get; set; }
    public string? Account_SortCode { get; set; }
    public string? Account_BuildingSocRef { get; set; }
    public string? FinalSubmission_BecauseSchemeCeased { get; set; }
    public DateOnly? FinalSubmission_DateSchemeCeased { get; set; }
    public string? FinalSubmission_ForYear { get; set; }
}
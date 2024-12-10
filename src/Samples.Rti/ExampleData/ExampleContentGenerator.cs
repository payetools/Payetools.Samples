// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;
using Payetools.Hmrc.Common.Rti.Model;
using Payetools.Hmrc.Rti.Factories;
using Payetools.Hmrc.Rti.Model;
using Payetools.Hmrc.Rti.Model.Core;
using Payetools.Payroll.Model;

namespace RtiExample.ExampleData;

internal static class ExampleContentGenerator
{
    public static IFpsBodyInputData GenerateSingleEmploymentRecord()
    {
        return new FpsBodyInputData()
        {
            // Common data
            PayeOfficeNumber = "635",
            PayeReference = "A635",
            AccountsOfficeRef = "635PH00386021",
            //CompanyTaxRef = "Z",

            // Employee section
            Title = "Mr",
            FirstName = "Stewart",
            MiddleName = "Christopher",
            LastName = "Williams",
            NiNumber = "NA123456C",
            AddressLine1 = "3 Osmond Drive",
            AddressLine2 = "Pilgrims Place",
            AddressLine3 = "Burntwood",
            AddressLine4 = "Essex",
            Postcode = "AN13 9LP",
            DateOfBirth = new DateOnly(1960, 7, 21),
            Gender = "M",
            DirectorsNi = "AL",
            PayrollId = "1234",
            TaxablePayYtd = 55066.12m,
            TotalTaxYtd = 10721.90m,
            BenefitsTaxedViaPayrollYtd = 1220.90m,
            PayFreq = "M1",
            PmtDate = new DateOnly(2023, 12, 20),
            HoursWorked = "A",
            TaxCode = "127L",
            TaxablePay = 1165.70m,
            DeductionsFromNetPay = 15.00m,
            PayAfterStatDeductions = 413.10m,
            BenefitsTaxedViaPayroll = 170.00m,
            EmpeePenContribnsNotPaidYtd = 101.08m,
            TaxDeductedOrRefunded = 294.12m,
            NiCategory_1 = NiCategory.A.ToString(),
            GrossEarningsForNicsInPeriod_1 = 994.87m,
            GrossEarningsForNicsYtd_1 = 49335.51m,
            AtLELYtd_1 = 6396.00m,
            LELtoPTYtd_1 = 5512.00m,
            PTtoUELYtd_1 = 24335.51m,
            TotalEmpNICInPeriod_1 = -513.40m,
            TotalEmpNICYtd_1 = 5846.22m,
            EmployeeContribnsInPd_1 = 116.10m,
            EmployeeContribnsYtd_1 = 4764.52m
        };
    }

    public static GovTalkMessage MakeGovTalkDocument(
        GovTalkMessageFactory govTalkMessageFactory,
        IRtiCredentials credentials,
        IPayRunDetails payRunDetails,
        IRheaderContact contact,
        IRheaderSenderType senderType,
        IEnumerable<IFpsBodyInputData> data)
    {
        RtiDocumentFactory factory = new RtiDocumentFactory(new TaxYear(TaxYearEnding.Apr5_2024));

        // The employer fields are repeated in each test entry
        var employer = MakeEmployer(data.First());

        var fps = factory.CreateFpsDocument();

        var fpsData = FullPaymentSubmissionTestDataFactory.MakeFullPaymentSubmissionData(
            payRunDetails,
            contact,
            senderType,
            employer,
            data);

        fps.Populate(fpsData);

        var message = govTalkMessageFactory.CreateMessage(
            RtiDocumentType.FullPaymentSubmission,
            RtiMessageFunction.Submission,
            new TransactionId(),
            credentials,
            TestSubmissionMode.TestGateway,
            employer.HmrcPayeReference ?? throw new ArgumentException("Invalid PAYE reference in test data", nameof(data)),
            "test@test.com",
            fps);

        return message;
    }

    private static IEmployer MakeEmployer(IFpsBodyInputData data) =>
        new TestEmployer(
            new HmrcPayeReference(
            int.Parse(data.PayeOfficeNumber), data.PayeReference),
            new HmrcAccountsOfficeReference(data.AccountsOfficeRef),
            data.CompanyTaxRef);
}
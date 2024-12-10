// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;
using Payetools.Hmrc.Rti;
using Payetools.Hmrc.Rti.Diagnostics;
using Payetools.Hmrc.Rti.Factories;
using Payetools.Hmrc.Rti.Model;
using Payetools.Hmrc.Rti.Model.Core;

namespace Payetools.Samples.Common.Rti;

public static class RtiSubmissionHelper
{
    public static async Task SubmitRtiDocumentAsync(
        ITransactionEngineClient transactionEngineClient,
        GovTalkMessageFactory govTalkMessageFactory,
        GovTalkMessage govTalkMessage,
        ILogger logger)
    {
        try
        {
            var submission = new RtiSubmission(
                new RtiSubmissionMonitor(logger),
                transactionEngineClient,
                govTalkMessageFactory,
                govTalkMessage);

            await submission.BeginSubmissionAsync();
        }
        catch (RtiSubmissionException ex)
        {
            logger.LogError(ex, "Submission failed immediately");

            switch (ex.SubmissionExceptionType)
            {
                case RtiSubmissionExceptionType.SingleError:
                    logger.LogError("Submission error: {error}", ex.Message);
                    break;

                case RtiSubmissionExceptionType.GovTalkError:
                    logger.LogError("Submission generated one or more GovTalkErrors: {message}", ex.Message);
                    logger.LogError("{errors}", string.Join("\r\n", ex.GovTalkErrors!.Select(gte => gte.ToString())));
                    break;

                case RtiSubmissionExceptionType.ErrorResponse:
                    logger.LogError("Error response: {mnessage}", ex.Message);
                    break;
            }
        }
    }
}

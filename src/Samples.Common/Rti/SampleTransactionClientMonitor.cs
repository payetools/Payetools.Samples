// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;
using Payetools.Hmrc.Rti.Model.Core;
using Payetools.Hmrc.Rti.Model.Monitoring;
using System.Xml.Linq;

namespace Payetools.Samples.Common.Rti;

public class SampleTransactionClientMonitor : IObserver<ITransactionEngineEvent>
{
    private readonly ILogger _logger;

    public SampleTransactionClientMonitor(ILogger logger)
    {
        _logger = logger;
    }

    public void OnCompleted()
    {
        _logger.LogInformation("Stream completed");
    }

    public void OnError(Exception error)
    {
        _logger?.LogError(error, "Stream errored");
    }

    public void OnNext(ITransactionEngineEvent value)
    {
        switch (value.EventType)
        {
            case TransactionEngineEventType.MessageSent:
                LogGovTalkMessage(value.Message, "TX");
                break;

            case TransactionEngineEventType.MessageReceived:
                LogGovTalkMessage(value.Message, "RX");
                break;

            case TransactionEngineEventType.Error:
                if (value.ClientError != null)
                {
                    _logger.LogInformation("ERROR: [{originalTransactionId}]", value.ClientError.OriginalTransactionId);
                    _logger.LogInformation("Details: {text} {errorData}", value.ClientError.ErrorText, value.ClientError.ErrorData ?? Array.Empty<object>());
                }
                break;
        }
    }

    private void LogGovTalkMessage(GovTalkMessage? message, string direction)
    {
        if (message?.MostRecentSerialisedMessage != null)
            _logger.LogInformation("{direction}: {message}", direction, XDocument.Parse(message.MostRecentSerialisedMessage).ToString());
    }
}
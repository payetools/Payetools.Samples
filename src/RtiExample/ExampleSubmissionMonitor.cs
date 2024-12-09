// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;
using Payetools.Hmrc.Rti.Model;
using Payetools.Hmrc.Rti.Model.Core;
using Payetools.Hmrc.Rti.Model.Monitoring;

namespace RtiExample;

internal class ExampleSubmissionMonitor : IRtiSubmissionMonitor
{
    private readonly ILogger _logger;

    public ExampleSubmissionMonitor(ILogger logger)
    {
        _logger = logger;
    }
    public Task NotifyErrorAsync(string? message, params object?[] args)
    {
        _logger.LogError(message, args);

        return Task.CompletedTask;
    }

    public Task NotifyInfoAsync(string? message, params object?[] args)
    {
        _logger.LogInformation(message, args);

        return Task.CompletedTask;
    }

    public Task NotifyProcessingCompletedAsync(RtiReceipt receipt, IRtiResponse response)
    {
        _logger.LogInformation(receipt.ToString());

        return Task.CompletedTask;
    }
}
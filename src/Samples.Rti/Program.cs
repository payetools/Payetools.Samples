// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;
using Payetools.Common.Model;
using Payetools.Hmrc.Common.Rti.Model;
using Payetools.Hmrc.Rti;
using Payetools.Hmrc.Rti.Factories;
using Payetools.Payroll.Model;
using Payetools.Samples.Common;
using Payetools.Samples.Common.Rti;
using RtiExample.ExampleData;

var logger = Logging.MakeLogger<Program>();

GovTalkMessageFactory govTalkMessageFactory = new GovTalkMessageFactory("0000", "Test Product", "1.0.0");

var creds = Environment.GetEnvironmentVariable("RTI_CREDENTIALS")?.Split(':', 2) ??
    throw new InvalidOperationException("Environment variable RTI_CREDENTIALS must be set in the format user:password");

var credentials = new RtiCredentials(creds[0], creds[1]);

var govTalkMessage = ExampleContentGenerator.MakeGovTalkDocument(
    govTalkMessageFactory,
    credentials,
    new PayRunDetails(new PayDate(2024, 1, 1, PayFrequency.Monthly), new DateRange(new DateOnly(2023, 12, 1), new DateOnly(2023, 12, 31))),
    new IRheaderContact(IRheaderContactType.None, new ContactName(["James"], "Hawkworth")),
    IRheaderSenderType.Company,
    [ExampleContentGenerator.GenerateSingleEmploymentRecord()]);

// NB Uses ExampleHttpClientFactory which initialises a specific HttpClientFactory for the HMRC connection, as HMRC uses
// GZip compression which is not enabled by default.
//
// Replace the URI below with new Uri("http://localhost:5665/LTS/LTSPostServlet") to test against the local test server.
using var transactionEngineClient = new TransactionEngineClient(
    new SampleHttpClientFactory(),
    new RtiTaskScheduler(),
    new Uri("https://test-transaction-engine.tax.service.gov.uk/submission"));

using var _ = transactionEngineClient.Subscribe(new SampleTransactionClientMonitor(logger));

await RtiSubmissionHelper.SubmitRtiDocumentAsync(
    transactionEngineClient,
    govTalkMessageFactory,
    govTalkMessage,
    logger);

// Wait around to allow the transaction engine to process the submission; results are displayed in the console
// via the ExampleTransactionClientMonitor created above
await Task.Delay(20000);

logger.LogInformation("Done");
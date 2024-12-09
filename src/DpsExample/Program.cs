// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using DpsExample;
using Payetools.Common.Model;
using Payetools.Hmrc.Common.Dps;
using Payetools.Hmrc.Dps;
using Payetools.Hmrc.Dps.Model;
using Payetools.Hmrc.Dps.Model.Messages;
using Payetools.Hmrc.Rti.Diagnostics;

var vendorId = "0000";

var creds = Environment.GetEnvironmentVariable("RTI_CREDENTIALS")?.Split(':', 2) ??
    throw new InvalidOperationException("Environment variable RTI_CREDENTIALS must be set in the format user:password");

var dpsCredentials = new DpsCredentials(creds[0], creds[1]);

// Initialise a specific HttpClientFactory for the HMRC connection, as HMRC uses GZip compression which is not enabled by default
var httpClientFactory = new TestHttpClientFactory();

// NB Use the following endpoints for the live HMRC service:
//    AuthEndpoint = new Uri("https://dps.ws.hmrc.gov.uk/dpsauthentication/service"),
//    RequestEndpoint = new Uri("https://dps.ws.hmrc.gov.uk/dps/service")
DpsEndpoints endpoints = new DpsEndpoints
{
    AuthEndpoint = new Uri("https://www.tpvs.hmrc.gov.uk/dpsauthentication/dpsauthentication.jws"),
    RequestEndpoint = new Uri("https://www.tpvs.hmrc.gov.uk/dps/dps.jws")
};

// var payeReference = HmrcPayeReference.Parse("123/A6");

var rawPayeReference = Environment.GetEnvironmentVariable("PAYE_REFERENCE") ??
    throw new InvalidOperationException("Environment variable RTI_CREDENTIALS must be set");

var payeReference = HmrcPayeReference.Parse(rawPayeReference);


IHmrcDpsConnection dpsConnection = new HmrcDpsConnection(
    vendorId,
    httpClientFactory,
    endpoints);

var token = await dpsConnection.Authenticate(payeReference, dpsCredentials);

var processor = new DpsMessageProcessor();

var highWaterMarks = new HighWaterMark[]
{
    new HighWaterMark(DpsMessageType.P6, 0),
    new HighWaterMark(DpsMessageType.P9, 0),
    new HighWaterMark(DpsMessageType.RTI, 0),
    new HighWaterMark(DpsMessageType.AR, 0),
    new HighWaterMark(DpsMessageType.SL1, 0),
    new HighWaterMark(DpsMessageType.SL2, 0),
    new HighWaterMark(DpsMessageType.PGL1, 0),
    new HighWaterMark(DpsMessageType.PGL2, 0),
    new HighWaterMark(DpsMessageType.NOT, 0)
};

try
{
    await dpsConnection.RetrieveMessages(processor, token, highWaterMarks);
}
catch (DpsProcessingException ex)
{
    Console.WriteLine(ex.Message);
}
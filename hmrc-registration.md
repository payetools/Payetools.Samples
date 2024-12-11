# Registering with HMRC as a software developer for PAYE

In order to use the HMRC test environments, you must first register with HMRC as a software developer
for PAYE.  The procedure is as follows.  Send an email along the following lines to the HMRC Software 
Developer Support Team (SDSTeam) at [SDSTeam@hmrc.gov.uk](mailto:SDSTeam@hmrc.gov.uk).

```
Dear SDSTeam,

We wish to register with HMRC as a software developer for PAYE as we are developing a commercial
product that includes support for running UK payroll. Our details are as follows:

Company name: <your company name>
Contact names: <at least one contact, but 2 or 3 if available>
Email addresses: <for contact name(s)>

We wish to support the following APIs:

- Real Time Information (RTI) XML API for making FPS, EPS and NVR submissions
- Outgoing Data Provisioning Service (DPS) XML API for retrieving notifications from HMRC

I can confirm we are familiar with the revelant documentation (Basic guide for XML software developers,
Software development for HMRC: detailed information, Real Time Information support for software developers,
PAYE Online support for software developers, Transaction Engine support for software developers).

Yours ...
```

HMRC should then add you to their list of software developers and provide you with the necessary information.
This will include:

- Vendor ID - this is a four -digit number that identifies your software to HMRC
- User ID and password for the HMRC test environment
- Test PAYE Reference - this is a dummy PAYE scheme that you can use for testing purposes

The credentials provided, along with the PAYE reference, will allow you to make test submissions to the
Third Party Validation Service (TPVS).  Note that in order to make "Test-in-Live" submissions to the
production HMRC Transaction Engine, you will need a valid Government Gateway ID and password that is
connected to a valid PAYE scheme (PAYE Reference).  (The easiest way to do this is to use your own
company's PAYE scheme, but care should be taken not to make production submissions to HMRC by mistake.)

HMRC will also ask you to register as a developer with the HMRC Developer Hub.  This is so you receive
notifications of changes to the APIs you are using.  You can register on the following page:
[Register for a developer account](https://developer.service.hmrc.gov.uk/developer/registration). If
more than one person in your company needs to receive notifications, you may wish to use a group email
address for this purpose.  Note that Payetools monitors changes to HMRC's PAYE APIs and updates its
libraries accordingly, so this step is optional.

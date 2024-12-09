// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Common.Model;
using Payetools.Payroll.Model;
using System.Net.Mail;

namespace RtiExample.ExampleData;

public class TestEmployee : IEmployee
{
    public NiNumber NiNumber { get; }

    public DateOnly DateOfBirth { get; }

    public Gender Gender { get; }

    public MailAddress? EmailAddress { get; set; }

    public PostalAddress PostalAddress { get; set; }

    public string? PassportNumber { get; set; }

    public IEmployeePartnerDetails? PartnerDetails { get; set; }

    public Title? Title { get; }

    public string? FirstName { get; }

    public char[]? Initials { get; }

    public string? MiddleNames { get; }

    public string LastName { get; }

    public string? KnownAsName { get; }

    public bool HasMiddleName => MiddleNames != null;

    public string? InitialsAsString => FirstName == null && Initials != null ? string.Join(", ", Initials) : null;

    public TestEmployee(
        NiNumber niNumber,
        DateOnly dateOfBirth,
        Gender gender,
        MailAddress? emailAddress,
        PostalAddress postalAddress,
        string? passportNumber,
        IEmployeePartnerDetails? partnerDetails,
        Title? title,
        string? firstName,
        char[]? initials,
        string? middleNames,
        string lastName,
        string? knownAsName)
    {
        NiNumber = niNumber;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        EmailAddress = emailAddress;
        PostalAddress = postalAddress;
        PassportNumber = passportNumber;
        PartnerDetails = partnerDetails;
        Title = title;
        FirstName = firstName;
        Initials = initials;
        MiddleNames = middleNames;
        LastName = lastName;
        KnownAsName = knownAsName;
    }
}

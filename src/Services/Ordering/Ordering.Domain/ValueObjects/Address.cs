namespace Ordering.Domain.ValueObjects;

public record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string? Email { get; } = default!;
    public string AddressLine { get; } = default!;
    public string City { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;
    public string Country { get; } = default!;

    protected Address() { }

    private Address(string firstName, string lastName, string email,
        string addressLine, string city, string state, string zipCode, string country)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AddressLine = addressLine;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    public static Address Of(string firstName, string lastName, string email,
        string addressLine, string city, string state, string zipCode, string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine, nameof(addressLine));

        return new Address
            (firstName, lastName, email, addressLine, city, state, zipCode, country);
    }
}

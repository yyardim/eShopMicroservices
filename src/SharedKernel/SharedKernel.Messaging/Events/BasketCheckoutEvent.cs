using SharedKernel.MessagingEvents;

namespace SharedKernel.Messaging.Events;

public record BasketCheckoutEvent : IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public List<BasketCheckoutItem> Items { get; set; } = [];

    // Shipping and Billing Address
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;

    // Payment Information
    public string CardNumber { get; set; } = default!;
    public string CardHolderName { get; set; } = default!;
    public string ExpirationDate { get; set; } = default!;
    public string Cvv { get; set; } = default!;
    public int PaymentMethod { get; set; }
}

public record BasketCheckoutItem(
    Guid ProductId,
    int Quantity,
    decimal Price);

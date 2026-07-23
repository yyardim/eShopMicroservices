namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string CardNumber { get; } = default!;
    public string CardHolderName { get; } = default!;
    public string ExpirationDate { get; } = default!;
    public string Cvv { get; } = default!;
    public int PaymentMethod { get; }

    protected Payment() { }

    private Payment(
        string cardNumber, string cardHolderName,
        string expirationDate, string cvv, int paymentMethod)
    {
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpirationDate = expirationDate;
        Cvv = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(
        string cardNumber, string cardHolderName,
        string expirationDate, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
        ArgumentException.ThrowIfNullOrWhiteSpace(cardHolderName, nameof(cardHolderName));
        ArgumentException.ThrowIfNullOrWhiteSpace(expirationDate, nameof(expirationDate));
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3, nameof(cvv));

        return new Payment
            (cardNumber, cardHolderName, expirationDate, cvv, paymentMethod);
    }
}

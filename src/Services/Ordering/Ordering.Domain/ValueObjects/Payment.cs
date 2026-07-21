namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string CardHolderName { get; } = default!;
    public string ExpirationDate { get; } = default!;
    public string Cvv { get; } = default!;
    public int PaymentMethod { get; } = default!;

    protected Payment() { }

    private Payment(
        string cardName, string cardNumber, string cardHolderName,
        string expirationDate, string cvv, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        ExpirationDate = expirationDate;
        Cvv = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(
        string cardName, string cardNumber, string cardHolderName,
        string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
        ArgumentException.ThrowIfNullOrWhiteSpace(cardHolderName, nameof(cardHolderName));
        ArgumentException.ThrowIfNullOrWhiteSpace(expiration, nameof(expiration));
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3, nameof(cvv));

        return new Payment
            (cardName, cardNumber, cardHolderName, expiration, cvv, paymentMethod);
    }
}

namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int MinLength = 2;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    public static OrderName Parse(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MinLength, nameof(value));

        return new OrderName(value);
    }
}

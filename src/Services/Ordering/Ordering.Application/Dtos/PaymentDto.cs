namespace Ordering.Application.Dtos;

public record PaymentDto(
    string CardNumber,
    string CardHolderName,
    string ExpirationDate,
    string Cvv,
    int PaymentMethod);

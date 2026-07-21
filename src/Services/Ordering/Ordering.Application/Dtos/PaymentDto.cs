namespace Ordering.Application.Dtos;

public record PaymentDto(
    string CardName,
    string CardNumber,
    string CardHolderName,
    string ExpirationDate,
    string Cvv,
    int PaymentMethod);

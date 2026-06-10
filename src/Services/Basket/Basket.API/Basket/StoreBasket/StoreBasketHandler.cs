namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        _ = RuleFor(static x => x.Cart).NotNull().WithMessage("Cart cannot be null!");
        _ = RuleFor(static x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty!");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle
        (StoreBasketCommand command, CancellationToken ct)
    {
        ShoppingCart cart = command.Cart;

        await repository.StoreBasket(cart, ct);

        return new StoreBasketResult(cart.UserName);
    }
}

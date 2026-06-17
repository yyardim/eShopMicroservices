using Discount.Grpc;

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

public class StoreBasketCommandHandler
    (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken ct)
    {
        await DeductDiscount(command, ct);

        _ = await repository.StoreBasket(command.Cart, ct);

        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(StoreBasketCommand command, CancellationToken ct)
    {
        foreach (ShoppingCartItem item in command.Cart.Items)
        {
            CouponModel coupon = await discountProto.GetDiscountAsync
                (new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: ct);
            item.Price -= coupon.Amount;
        }
    }
}

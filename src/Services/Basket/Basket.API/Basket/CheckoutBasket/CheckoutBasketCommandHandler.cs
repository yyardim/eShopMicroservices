using MassTransit;
using SharedKernel.Messaging.Events;

namespace Basket.API.Basket.CheckoutBasket;

public record BasketCheckoutCommand(BasketCheckoutDto BasketCheckoutDto)
    : ICommand<BasketCheckoutResult>;
public record BasketCheckoutResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<BasketCheckoutCommand>
{
    public CheckoutBasketCommandValidator()
    {
        _ = RuleFor(static x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto cannot be null!");
        _ = RuleFor(static x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName cannot be empty!");
    }
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<BasketCheckoutCommand, BasketCheckoutResult>
{
    public async Task<BasketCheckoutResult> Handle(BasketCheckoutCommand command, CancellationToken ct)
    {
        ShoppingCart basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, ct);

        BasketCheckoutEvent eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = Convert.ToDecimal(basket.TotalPrice);
        eventMessage.Items = [.. basket.Items.Select(static item => new BasketCheckoutItem(
            item.ProductId,
            item.Quantity,
            Convert.ToDecimal(item.Price)))];

        await publishEndpoint.Publish(eventMessage, cancellationToken: ct);

        _ = await repository.DeleteBasket(basket.UserName, ct);

        return new BasketCheckoutResult(true);
    }
}

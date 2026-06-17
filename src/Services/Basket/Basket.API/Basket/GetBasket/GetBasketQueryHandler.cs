namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);
public class GetBasketQueryHandler
    (IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken ct)
    {
        ShoppingCart basket = await repository.GetBasket(request.UserName, ct);
        
        return new GetBasketResult(basket);
    }
}

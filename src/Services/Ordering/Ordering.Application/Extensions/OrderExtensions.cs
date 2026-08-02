namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(static order => order.ToOrderDto());
    }

    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto(
            Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto(
                FirstName: order.ShippingAddress.FirstName,
                LastName: order.ShippingAddress.LastName,
                Email: order.ShippingAddress.Email,
                AddressLine: order.ShippingAddress.AddressLine,
                City: order.ShippingAddress.City,
                State: order.ShippingAddress.State,
                ZipCode: order.ShippingAddress.ZipCode,
                Country: order.ShippingAddress.Country
            ),
            BillingAddress: new AddressDto(
                FirstName: order.BillingAddress.FirstName,
                LastName: order.BillingAddress.LastName,
                Email: order.BillingAddress.Email,
                AddressLine: order.BillingAddress.AddressLine,
                City: order.BillingAddress.City,
                State: order.BillingAddress.State,
                ZipCode: order.BillingAddress.ZipCode,
                Country: order.BillingAddress.Country
            ),
            Payment: new PaymentDto(
                CardNumber: order.Payment.CardNumber,
                CardHolderName: order.Payment.CardHolderName,
                ExpirationDate: order.Payment.ExpirationDate,
                Cvv: order.Payment.Cvv,
                PaymentMethod: order.Payment.PaymentMethod
            ),
            Status: order.Status,
            OrderItems: [.. order.OrderItems.Select(static oi => new OrderItemDto(
                OrderId: oi.OrderId.Value,
                ProductId: oi.ProductId.Value,
                Quantity: oi.Quantity,
                Price: oi.Price))]
        );
    }
}

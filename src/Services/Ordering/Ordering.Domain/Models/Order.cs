namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalAmount
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    public static Order Create(
        OrderId id,
        CustomerId customerId,
        OrderName orderName,
        Address shippingAddress,
        Address billingAddress,
        Payment payment)
    {
        Order order = new()
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending,
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress,
        Payment payment, OrderStatus status)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void Add(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        OrderItem orderItem = new(Id, productId, quantity, price);
        _orderItems.Add(orderItem);

        //AddDomainEvent(new OrderItemAddedEvent(this, orderItem));
    }

    public void Remove(ProductId product)
    {
        OrderItem? orderItem = _orderItems.FirstOrDefault(x => x.ProductId == product);
        if (orderItem is not null)
        {
            _orderItems.Remove(orderItem);
            //AddDomainEvent(new OrderItemRemovedEvent(this, orderItem));
        }
    }
}

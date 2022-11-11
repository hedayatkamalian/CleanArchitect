using CleanArchitect.Domain.ValueObjects;

namespace CleanArchitect.Domain.Entities;

public class Order
{
    public long Id { get; private set; }
    public long CustomerId { get; private set; }
    public virtual Customer Customer { get; private set; }
    public IList<OrderItem>? Items { get; private set; }
    public decimal TotalPrice { get { return Items?.Sum(p => p.TotalPrice) ?? 0; } }
    public Address Address { get; private set; }


    public static Order New
        (long id,
        long customerId,
        IList<OrderItem> items,
        Address address)
    {
        return new Order
        {
            Id = id,
            CustomerId = customerId,
            Items = items,
            Address = address
        };
    }


    public void Edit
        (long customerId,
        IList<OrderItem> items,
        Address address)
    {
        CustomerId = customerId;
        Items.Clear();
        AddItems(items);
        Address = address;
    }


    public void AddItems(IList<OrderItem> items)
    {
        //Items ??= new List<OrderItem>();
        ((List<OrderItem>)Items).AddRange(items);
    }
}

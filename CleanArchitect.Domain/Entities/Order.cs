using CleanArchitect.Domain.ValueObjects;

namespace CleanArchitect.Domain.Entities;

public class Order
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public IList<OrderItem> Items { get; set; }
    public decimal TotalPrice { get { return Items.Sum(p => p.TotalPrice); } }
    public Address Address { get; set; }
}

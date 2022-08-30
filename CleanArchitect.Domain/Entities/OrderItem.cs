namespace CleanArchitect.Domain.Entities;

public class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get { return Quantity * (Price - Discount); } }
    public int Quantity { get; set; }

}

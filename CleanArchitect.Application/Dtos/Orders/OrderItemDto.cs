namespace CleanArchitect.Application.Dtos.Orders;

public class OrderItemDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

    public int Quantity { get; set; }
    public decimal TotalPrice
    {
        get { return Quantity * (Price - Discount); }
    }
}

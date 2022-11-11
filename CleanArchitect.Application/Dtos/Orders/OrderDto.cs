namespace CleanArchitect.Application.Dtos.Orders;
public class OrderDto
{
    public long Id { get; set; }
    public long Customerid { get; set; }
    public string CustomerName { get; set; }
    public AddressDto Address { get; set; }
    public IList<OrderItemDto> Items { get; set; }
    public decimal TotalPrice
    {
        get { return Items.Sum(p => p.TotalPrice); }
    }
}

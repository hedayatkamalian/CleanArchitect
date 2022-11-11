namespace CleanArchitect.Domain.Commands.Orders
{
    public class OrderItemAddCommand
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}

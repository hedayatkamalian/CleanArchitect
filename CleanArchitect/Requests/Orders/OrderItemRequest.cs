namespace CleanArchitect.Requests.Orders
{
    public class OrderItemRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}

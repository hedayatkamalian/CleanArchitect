using CleanArchitect.Requests.General;

namespace CleanArchitect.Requests.Orders
{
    public class OrderAddRequest
    {
        public long CustomerId { get; set; }
        public IList<OrderItemRequest> Items { get; set; }
        public AddressRequest Address { get; set; }
    }
}

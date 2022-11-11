using CleanArchitect.Domain.ValueObjects;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Orders
{
    public class OrderEditCommand : IRequest<ServiceCommandResult>
    {
        public OrderEditCommand(long id, long customerId, Address address, IList<OrderItemAddCommand> items)
        {
            Id = id;
            CustomerId = customerId;
            Items = items;
            Address = address;
        }

        public long Id { get; private set; }
        public long CustomerId { get; private set; }
        public IList<OrderItemAddCommand> Items { get; private set; }
        public Address Address { get; set; }
    }
}

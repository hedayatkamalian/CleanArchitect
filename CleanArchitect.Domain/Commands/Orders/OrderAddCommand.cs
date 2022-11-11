using CleanArchitect.Domain.ValueObjects;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Orders
{
    public class OrderAddCommand : IRequest<ServiceCommandResult>
    {
        public OrderAddCommand(long customerId, Address address, IList<OrderItemAddCommand> items)
        {
            CustomerId = customerId;
            Address = address;
            Items = items;
        }

        public long CustomerId { get; private set; }
        public Address Address { get; private set; }
        public IList<OrderItemAddCommand> Items { get; private set; }
    }
}

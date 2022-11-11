using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Orders
{
    public class OrderDeleteCommand : IRequest<ServiceCommandResult>
    {
        public OrderDeleteCommand(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }


    }
}

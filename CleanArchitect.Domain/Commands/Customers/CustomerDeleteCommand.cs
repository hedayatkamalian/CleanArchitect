using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Customers
{
    public class CustomerDeleteCommand : IRequest<ServiceCommandResult>
    {
        public long Id { get; private set; }

        public CustomerDeleteCommand(long id)
        {
            Id = id;
        }
    }
}

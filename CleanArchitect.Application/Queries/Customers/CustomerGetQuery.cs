using CleanArchitect.Application.Dtos.Customers;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Customers
{
    public class CustomerGetQuery : IRequest<ServiceQueryResult<CustomerDto>>
    {
        public CustomerGetQuery(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}

using CleanArchitect.Application.Dtos.Customers;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Customers
{
    public class CustomerGetAllQuery : IRequest<ServiceQueryResult<IList<CustomerDto>>>
    {
    }
}

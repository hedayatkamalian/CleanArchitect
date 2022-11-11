using CleanArchitect.Application.Dtos.Orders;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Orders
{
    public class OrderGetAllQuery : IRequest<ServiceQueryResult<IList<OrderDto>>>
    {
    }
}

using CleanArchitect.Application.Dtos.Orders;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Orders
{
    public class OrderGetQuery : IRequest<ServiceQueryResult<OrderDto>>
    {
        public OrderGetQuery(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}

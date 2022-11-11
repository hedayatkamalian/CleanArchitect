using CleanArchitect.Application.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Orders.GetAll
{
    [Controller]
    [Route("[Controller]")]
    public class OrdersController : CustomController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new OrderGetAllQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return FromServiceResult(result);
        }
    }
}

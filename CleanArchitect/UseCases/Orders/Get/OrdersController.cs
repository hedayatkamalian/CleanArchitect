using CleanArchitect.Application.Queries.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitect.UseCases.Orders.Get
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Required][FromRoute] long id, CancellationToken cancellationToken)
        {
            var query = new OrderGetQuery(id);

            var result = await _mediator.Send(query, cancellationToken);

            return FromServiceResult(result);
        }
    }
}

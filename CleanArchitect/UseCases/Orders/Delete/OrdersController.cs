using CleanArchitect.Domain.Commands.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitect.UseCases.Orders.Delete
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Required][FromRoute] long id, CancellationToken cancellationToken)
        {
            var command = new OrderDeleteCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return FromServiceResult(result);
        }
    }
}

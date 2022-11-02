using CleanArchitect.Domain.Commands.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Customers.Delete
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : CustomController
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            var command = new CustomerDeleteCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return FromServiceResult(result);
        }
    }
}

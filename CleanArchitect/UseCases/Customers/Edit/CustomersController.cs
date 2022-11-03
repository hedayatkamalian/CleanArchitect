using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Requests.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Customers.Edit
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] CustomersEditRequest request, CancellationToken cancellationToken)
        {
            var command = new CustomerEditCommand(id, request.FirstName, request.LastName, request.PhoneNumber);
            var result = await _mediator.Send(command, cancellationToken);
            return FromServiceResult(result);
        }
    }
}

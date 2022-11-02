using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Requests.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Customers.Add
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Add([FromBody] CustomersAddRequest request, CancellationToken cancellationToken)
        {
            var command = new CustomerAddCommand(request.FirstName, request.LastName, request.PhoneNumber);
            var result = await _mediator.Send(command, cancellationToken);

            const string locationActionName = nameof(Get.CustomersController.Get);
            result.Uri = $"{Url.Action(locationActionName, new { id = result.Id })}";

            return FromServiceResult(result);
        }
    }
}

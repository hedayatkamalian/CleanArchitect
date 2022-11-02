using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Queries.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Customers.Get
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
        {
            var query = new CustomerGetQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return FromServiceResult(result);
        }
    }
}

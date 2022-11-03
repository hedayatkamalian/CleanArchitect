using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Queries.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Customers.GetAll
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

        [HttpGet]
        [ProducesResponseType(typeof(IList<CustomerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new CustomerGetAllQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return FromServiceResult(result);
        }
    }
}

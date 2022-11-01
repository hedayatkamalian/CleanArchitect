using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Requests.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Products.Add
{
    [Route("[controller]")]
    [Controller]
    public class ProductsController : CustomController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Uri), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddRequest request, CancellationToken cancellationToken)
        {
            var command = new ProductAddCommand(request.Name, request.Price);
            var result = await _mediator.Send(command, cancellationToken);

            const string locationActionName = nameof(Get.ProductsController.Get);
            result.Uri = $"{Url.Action(locationActionName, new { id = result.Id })}";

            return FromServiceResult(result);
        }
    }
}

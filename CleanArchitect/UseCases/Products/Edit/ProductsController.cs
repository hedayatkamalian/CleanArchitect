using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Requests.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Products.Edit
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : CustomController
    {
        private readonly IMediator _mediatR;

        public ProductsController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] ProductEditRequest request)
        {
            var command = new ProductEditCommand(id, request.Name, request.Price);
            var result = await _mediatR.Send(command);
            return FromServiceResult(result);
        }
    }
}

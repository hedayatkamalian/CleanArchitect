using CleanArchitect.Domain.Commands.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Products.Delete
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            var command = new ProductDeleteCommand(id);
            var result = await _mediatR.Send(command, cancellationToken);
            return FromServiceResult(result);
        }
    }
}

using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Products.Get
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
        {
            var query = new ProductGetQuery(id);
            var result = await _mediatR.Send(query, cancellationToken);
            return FromServiceResult(result);

        }
    }
}

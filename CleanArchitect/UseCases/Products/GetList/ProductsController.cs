using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Products.GetList
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

        [HttpGet]
        [ProducesResponseType(typeof(IList<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new ProductGetListQuery();
            var result = await _mediatR.Send(query, cancellationToken);
            return FromServiceResult(result);

        }
    }
}

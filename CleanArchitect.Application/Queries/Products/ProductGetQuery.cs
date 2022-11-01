using CleanArchitect.Application.Dtos.Products;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Products;

public class ProductGetQuery : IRequest<ServiceQueryResult<ProductDto>>
{
    public ProductGetQuery(long productId)
    {
        ProductId = productId;
    }

    public long ProductId { get; set; }
}

using CleanArchitect.Application.Dtos.Products;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Products;

public class ProductGetListQuery : IRequest<ServiceQueryResult<IList<ProductDto>>>
{
}

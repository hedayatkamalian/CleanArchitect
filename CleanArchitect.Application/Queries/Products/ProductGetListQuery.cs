using CleanArchitect.Application.Dtos.Products;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.Queries.Products;

public class ProductGetAllQuery : IRequest<ServiceQueryResult<IList<ProductDto>>>
{
}

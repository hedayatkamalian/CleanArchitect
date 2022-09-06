using AutoMapper;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Products;
using CleanArchitect.Domain.Repositories;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Products.QueryHandlers;

public class ProductGetQueryHandler : IRequestHandler<ProductGetQuery, ServiceQueryResult<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ServiceQueryResult<ProductDto>> Handle(ProductGetQuery query, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(query.ProductId, cancellationToken);
        var productDto = product is not null ? _mapper.Map<ProductDto>(product) : null;
        return new ServiceQueryResult<ProductDto>(productDto);

    }
}

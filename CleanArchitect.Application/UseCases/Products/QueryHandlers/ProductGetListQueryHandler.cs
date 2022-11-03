using AutoMapper;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Options;
using CleanArchitect.Application.Queries.Products;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Products.QueryHandlers;

public class ProductGetAllQueryHandler : IRequestHandler<ProductGetAllQuery, ServiceQueryResult<IList<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApplicationErrors _applicationErrros;

    public ProductGetAllQueryHandler(IUnitOfWork unitOfWork,
        IOptions<ApplicationErrors> applicationErrros,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _applicationErrros = applicationErrros.Value;
    }

    public async Task<ServiceQueryResult<IList<ProductDto>>> Handle(ProductGetAllQuery query, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync(cancellationToken);
        return new ServiceQueryResult<IList<ProductDto>>(_mapper.Map<IList<ProductDto>>(products));
    }
}

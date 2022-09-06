using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Products.Commands;

public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationErrors _applicationErrors;

    public ProductAddCommandHandler(IUnitOfWork unitOfWork,
        IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _applicationErrors = applicationErrors.Value;
    }

    public async Task<ServiceCommandResult> Handle(ProductAddCommand request, CancellationToken cancellationToken)
    {

        var product = new Product
        {
            Id = new IdGen.IdGenerator(0).CreateId(),
            Name = request.Name,
            Price = request.Price,
        };

        await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result > 0 ?
            new ServiceCommandResult(product.Id.ToString()) :
            new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
    }
}

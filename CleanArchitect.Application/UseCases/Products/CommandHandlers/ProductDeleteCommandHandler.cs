using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Products.CommandHandlers;

public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationErrors _applicationErrors;

    public ProductDeleteCommandHandler(IUnitOfWork unitOfWork,
        IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _applicationErrors = applicationErrors.Value;
    }
    public async Task<ServiceCommandResult> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(request.Id, cancellationToken);

        if (product is null)
            return ServiceCommandResult.NotFound();

        await _unitOfWork.ProductRepository.DeleteAsync(product.Id, cancellationToken);

        var deleteResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return deleteResult > 0 ? ServiceCommandResult.Done() : ServiceCommandResult.Error(_applicationErrors.NoRowsWereAffected);

    }
}

using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Products.CommandHandlers;

public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationErrors _applicationErrors;

    public ProductEditCommandHandler(IUnitOfWork unitOfWork,
        IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _applicationErrors = applicationErrors.Value;
    }

    public async Task<ServiceCommandResult> Handle(ProductEditCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(request.Id, cancellationToken);

        if (product is null)
            return ServiceCommandResult.NotFound();

        product.Price = request.Price;
        product.Name = request.Name;

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return saveResult > 0 ? ServiceCommandResult.Done() : ServiceCommandResult.Error(_applicationErrors.NoRowsWereAffected);
    }
}

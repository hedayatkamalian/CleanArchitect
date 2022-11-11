using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Orders;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Orders.CommandHandlers;

public class OrderDeleteCommandHandler : IRequestHandler<OrderDeleteCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationErrors _applicationErrors;

    public OrderDeleteCommandHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _applicationErrors = applicationErrors.Value;
    }

    public async Task<ServiceCommandResult> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetAsync(request.Id, cancellationToken);

        if (order is null)
            return new ServiceCommandResult(CommandErrorType.NotFound);

        await _unitOfWork.OrderRepository.DeleteAsync(order.Id, cancellationToken);
        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return saveResult > 0 ? new ServiceCommandResult() : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
    }
}

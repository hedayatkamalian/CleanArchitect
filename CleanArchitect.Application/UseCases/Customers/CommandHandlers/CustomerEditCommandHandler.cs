using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Customers.CommandHandlers;

public class CustomerEditCommandHandler : IRequestHandler<CustomerEditCommand, ServiceCommandResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationErrors _applicationErrors;

    public CustomerEditCommandHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors)
    {
        _unitOfWork = unitOfWork;
        _applicationErrors = applicationErrors.Value;
    }

    public async Task<ServiceCommandResult> Handle(CustomerEditCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            return new ServiceCommandResult(CommandErrorType.NotFound);
        }

        if (
            string.IsNullOrEmpty(request.FirstName.Trim())
            || string.IsNullOrEmpty(request.LastName.Trim())
            || string.IsNullOrEmpty(request.PhoneNumber.Trim())
            )
        {
            return new ServiceCommandResult(CommandErrorType.Validation);
        }


        customer.Edit(request.FirstName, request.LastName, request.PhoneNumber);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result > 0 ? new ServiceCommandResult() : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
    }
}

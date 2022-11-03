using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Customers.CommandHandlers
{
    public class CustomerDeleteCommandHandler : IRequestHandler<CustomerDeleteCommand, ServiceCommandResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationErrors _applicationErrors;

        public CustomerDeleteCommandHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors)
        {
            _unitOfWork = unitOfWork;
            _applicationErrors = applicationErrors.Value;
        }

        public async Task<ServiceCommandResult> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetAsync(request.Id, cancellationToken);

            if (customer is null)
            {
                return new ServiceCommandResult(CommandErrorType.NotFound);
            }


            await _unitOfWork.CustomerRepository.DeleteAsync(customer.Id, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0 ? new ServiceCommandResult() : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
        }
    }
}

using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Customers.CommandHandlers
{
    public class CustomerAddCommandHandler : IRequestHandler<CustomerAddCommand, ServiceCommandResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationErrors _applicationErrors;

        public CustomerAddCommandHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors)
        {
            _unitOfWork = unitOfWork;
            _applicationErrors = applicationErrors.Value;
        }

        public async Task<ServiceCommandResult> Handle(CustomerAddCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FirstName.Trim())
                || string.IsNullOrEmpty(request.LastName.Trim())
                || string.IsNullOrEmpty(request.PhoneNumber.Trim()))
            {
                return new ServiceCommandResult(CommandErrorType.Validation);
            }


            var customer = Customer.New(
                new IdGen.IdGenerator(0).CreateId(),
                request.FirstName,
                request.LastName,
                request.PhoneNumber);

            await _unitOfWork.CustomerRepository.AddAsync(customer, cancellationToken);

            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return saveResult > 0 ? new ServiceCommandResult(customer.Id.ToString()) : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);
        }
    }
}

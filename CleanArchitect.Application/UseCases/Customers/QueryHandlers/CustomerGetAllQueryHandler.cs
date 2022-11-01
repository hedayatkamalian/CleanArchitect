using AutoMapper;
using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Queries.Customers;
using CleanArchitect.Domain.Repositories;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Customers.QueryHandlers
{
    public class CustomerGetAllQueryHandler : IRequestHandler<CustomerGetAllQuery, ServiceQueryResult<IList<CustomerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceQueryResult<IList<CustomerDto>>> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllAsync(cancellationToken);
            var customersDto = _mapper.Map<IList<CustomerDto>>(customers);

            return new ServiceQueryResult<IList<CustomerDto>>(customersDto);
        }
    }
}

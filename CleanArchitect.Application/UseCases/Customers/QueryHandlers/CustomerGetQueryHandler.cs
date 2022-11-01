using AutoMapper;
using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Queries.Customers;
using CleanArchitect.Domain.Repositories;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Customers.QueryHandlers
{
    public class CustomerGetQueryHandler : IRequestHandler<CustomerGetQuery, ServiceQueryResult<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceQueryResult<CustomerDto>> Handle(CustomerGetQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.CustomerRepository.GetAsync(request.Id, cancellationToken);

            if (customer is null)
            {
                return new ServiceQueryResult<CustomerDto>(null as CustomerDto);
            }

            return new ServiceQueryResult<CustomerDto>(_mapper.Map<CustomerDto>(customer));

        }
    }
}

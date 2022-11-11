using AutoMapper;
using CleanArchitect.Application.Dtos.Orders;
using CleanArchitect.Application.Options;
using CleanArchitect.Application.Queries.Orders;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Orders.QueryHandlers
{
    public class OrderGetQueryHandler : IRequestHandler<OrderGetQuery, ServiceQueryResult<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationErrors _applicationErrors;

        public OrderGetQueryHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationErrors = applicationErrors.Value;
        }
        public async Task<ServiceQueryResult<OrderDto>> Handle(OrderGetQuery request, CancellationToken cancellationToken)
        {

            var order = await _unitOfWork.OrderRepository.GetAsync(request.Id, cancellationToken);

            if (order is null)
                return new ServiceQueryResult<OrderDto>(null as OrderDto);

            return new ServiceQueryResult<OrderDto>(_mapper.Map<OrderDto>(order));

        }
    }
}

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
    public class OrderGetAllQueryHandler : IRequestHandler<OrderGetAllQuery, ServiceQueryResult<IList<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationErrors _applicationErrors;

        public OrderGetAllQueryHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationErrors = applicationErrors.Value;
        }
        public async Task<ServiceQueryResult<IList<OrderDto>>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {

            var orders = await _unitOfWork.OrderRepository.GetAllAsync(cancellationToken);
            return new ServiceQueryResult<IList<OrderDto>>(_mapper.Map<IList<OrderDto>>(orders));

        }
    }
}

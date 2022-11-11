using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Commands.Orders;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Application.UseCases.Orders.CommandHandlers
{
    public class OrderEditCommandHandler : IRequestHandler<OrderEditCommand, ServiceCommandResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationErrors _applicationErrors;

        public OrderEditCommandHandler(IUnitOfWork unitOfWork, IOptions<ApplicationErrors> applicationErrors)
        {
            _unitOfWork = unitOfWork;
            _applicationErrors = applicationErrors.Value;
        }

        public async Task<ServiceCommandResult> Handle(OrderEditCommand request, CancellationToken cancellationToken)
        {



            if (request.Items.Count == 0)
                return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.OrderAtLeastShouldHasOneOrderItem);

            if (request.Items.Any(p => p.Quantity <= 0))
            {
                return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.OrderItemsQuantityShouldBeGreaterThanZero);
            }


            if (request.Items.Count != request.Items.Select(p => p.ProductId).Distinct().Count())
                return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.OrderItemsAreNotUnique);

            var order = await _unitOfWork.OrderRepository.GetAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return new ServiceCommandResult(CommandErrorType.NotFound);
            }

            var customer = await _unitOfWork.CustomerRepository.GetAsync(request.CustomerId, cancellationToken);
            if (customer == null)
                return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.CustomerNotFound);



            var productIdList = request.Items.Select(x => x.ProductId).ToList();
            var products = await _unitOfWork.ProductRepository.GetAllAsync(p => productIdList.Contains(p.Id), cancellationToken);

            if (products.Count < request.Items.Count)
            {
                return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.SomeOfOrderItemsDoesNotExist);
            }



            var orderItems = new List<OrderItem>();
            var idGenerator = new IdGen.IdGenerator(0);

            foreach (var item in request.Items)
            {
                var selectedProduct = products.Where(p => p.Id == item.ProductId).FirstOrDefault();
                if (selectedProduct.Price < item.Discount)
                {
                    return new ServiceCommandResult(CommandErrorType.Validation, _applicationErrors.ItemDiscountShouldNotBeBiggerThanPrice);
                }



                orderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    Price = selectedProduct.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Id = idGenerator.CreateId(),
                    Discount = item.Discount
                });
            }

            order.Edit(request.CustomerId, orderItems, request.Address);
            await _unitOfWork.OrderRepository.UpdateAsync(order);

            var saveResutl = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return saveResutl > 0 ? new ServiceCommandResult() : new ServiceCommandResult(CommandErrorType.General, _applicationErrors.NoRowsWereAffected);

        }
    }
}

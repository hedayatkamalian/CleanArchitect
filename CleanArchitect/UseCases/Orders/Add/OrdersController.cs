using CleanArchitect.Domain.Commands.Orders;
using CleanArchitect.Domain.ValueObjects;
using CleanArchitect.Requests.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Orders.Add
{
    [Controller]
    [Route("[Controller]")]
    public class OrdersController : CustomController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderAddRequest request, CancellationToken cancellationToken)
        {
            var address = new Address
            {
                City = request.Address.City,
                Country = request.Address.Country,
                Street = request.Address.Street,
                State = request.Address.State,
                No = request.Address.No
            };

            var orderAddItems = new List<OrderItemAddCommand>();

            foreach (var item in request.Items)
            {
                orderAddItems.Add(new OrderItemAddCommand
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Discount = item.Discount
                });
            }

            var command = new OrderAddCommand(request.CustomerId, address, orderAddItems);



            var result = await _mediator.Send(command, cancellationToken);

            const string locationActionName = nameof(Get.OrdersController.Get);
            result.Uri = $"{Url.Action(locationActionName, new { id = result.Id })}";


            return FromServiceResult(result);
        }
    }
}

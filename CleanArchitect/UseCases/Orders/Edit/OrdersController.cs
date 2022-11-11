using CleanArchitect.Domain.Commands.Orders;
using CleanArchitect.Domain.ValueObjects;
using CleanArchitect.Requests.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.WebApi;

namespace CleanArchitect.UseCases.Orders.Edit
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] OrderEditRequest request, CancellationToken cancellationToken)
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

            var command = new OrderEditCommand(id, request.CustomerId, address, orderAddItems);

            var result = await _mediator.Send(command, cancellationToken);

            return FromServiceResult(result);
        }
    }
}

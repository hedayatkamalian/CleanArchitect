using AutoFixture;
using CleanArchitect.Application.Dtos.Orders;
using CleanArchitect.Application.Queries.Orders;
using CleanArchitect.Domain.Commands.Orders;
using CleanArchitect.Requests.Orders;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Domain.Abstraction;




namespace CleanArchitect.Tests.WebControllers
{

    public class OrdersControllerTests
    {
        private Fixture _fixtrue;

        public OrdersControllerTests()
        {
            _fixtrue = new Fixture();
        }

        [Fact]
        public async Task Add_Should_Return_Create_Result_When_CommandResult_Is_Successfull()
        {
            var serviceResult = new ServiceCommandResult(_fixtrue.Create<string>());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var ordersController = new UseCases.Orders.Add.OrdersController(mediatorMock.Object);
            ordersController.Url = GeneralFixtures.CreateMockUrlHelper().Object;
            var result = await ordersController.Add(_fixtrue.Create<OrderAddRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();
            ((CreatedResult)result).Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Add_Shoud_Return_UnprocessableEntity_When_CommandResult_Has_Validation_Error()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult(CommandErrorType.Validation);

            mediatorMock.Setup(m => m.Send(It.IsAny<OrderAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var ordersController = new UseCases.Orders.Add.OrdersController(mediatorMock.Object);
            ordersController.Url = GeneralFixtures.CreateMockUrlHelper().Object;

            var result = await ordersController.Add(_fixtrue.Create<OrderAddRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntityObjectResult>();

        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_CommandResult_Has_NotFound_Error()
        {
            var commandResutl = new ServiceCommandResult(CommandErrorType.NotFound);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var ordersController = new UseCases.Orders.Delete.OrdersController(mediatorMock.Object);
            var result = await ordersController.Delete(_fixtrue.Create<long>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_should_Return_NoContent_when_CommandResult_Has_No_Error()
        {
            var commandResutl = new ServiceCommandResult();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var ordersController = new UseCases.Orders.Delete.OrdersController(mediatorMock.Object);
            var result = await ordersController.Delete(_fixtrue.Create<long>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task Edit_Should_Return_NotFound_When_CommandResult_Has_NotFound_Error()
        {
            var commandResutl = new ServiceCommandResult(CommandErrorType.NotFound);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var ordersController = new UseCases.Orders.Edit.OrdersController(mediatorMock.Object);
            var result = await ordersController.Edit(_fixtrue.Create<long>(), _fixtrue.Create<OrderEditRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Edit_should_Return_NoContent_when_CommandResult_Has_No_Error()
        {
            var commandResutl = new ServiceCommandResult();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var ordersController = new UseCases.Orders.Edit.OrdersController(mediatorMock.Object);
            var result = await ordersController.Edit(_fixtrue.Create<long>(), _fixtrue.Create<OrderEditRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task Get_Should_Return_NotFound_When_QueryResult_Has_NotFound_Error()
        {
            var queryResutl = new ServiceQueryResult<OrderDto>(null as OrderDto);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderGetQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResutl);

            var ordersController = new UseCases.Orders.Get.OrdersController(mediatorMock.Object);
            var result = await ordersController.Get(_fixtrue.Create<long>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_Should_Return_OK_When_QueryResult_Has_No_Error_With_Same_Id()
        {
            var id = _fixtrue.Create<long>();
            var queryResutl = new ServiceQueryResult<OrderDto>(new OrderDto { Id = id });

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<OrderGetQuery>(p => p.Id == id), It.IsAny<CancellationToken>())).ReturnsAsync(queryResutl);

            var ordersController = new UseCases.Orders.Get.OrdersController(mediatorMock.Object);
            var result = await ordersController.Get(id, _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().BeOfType<OrderDto>();
            ((OrderDto)((OkObjectResult)result).Value).Id.Should().Be(id);
        }

        [Fact]
        public async Task GetList_Should_Return_OK_Always()
        {
            var queryListResult = new ServiceQueryResult<IList<OrderDto>>(new List<OrderDto>());


            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<OrderGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryListResult);

            var ordersController = new UseCases.Orders.GetAll.OrdersController(mediatorMock.Object);
            var result = await ordersController.GetAll(_fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}

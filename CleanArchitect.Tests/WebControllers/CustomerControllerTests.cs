using AutoFixture;
using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Queries.Customers;
using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Requests.Customers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Tests.WebControllers
{
    public class CustomerControllerTests
    {
        private Fixture _fixture { get { return new Fixture(); } }
        public CustomerControllerTests()
        {

        }

        [Fact]
        public async Task Add_Should_Return_Unsupportable_Entity_When_Command_Result_Has_Validation_Error()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult(CommandErrorType.Validation);
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Add.CustomersController(mediatorMock.Object);
            customerController.Url = GeneralFixtures.CreateMockUrlHelper().Object;
            var result = await customerController.Add(_fixture.Create<CustomersAddRequest>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
        }

        [Fact]
        public async Task Add_Should_Return_NoContent_When_Command_Result_Is_Successfull()
        {
            var serviceResult = new ServiceCommandResult(_fixture.Create<string>());
            serviceResult.Uri = _fixture.Create<string>();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Add.CustomersController(mediatorMock.Object);
            customerController.Url = GeneralFixtures.CreateMockUrlHelper().Object;
            var result = await customerController.Add(_fixture.Create<CustomersAddRequest>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Edit_Should_Return_Unsupportable_Entity_When_Command_Result_Has_Validation_Error()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult(CommandErrorType.Validation);
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Edit.CustomersController(mediatorMock.Object);
            var result = await customerController.Edit(_fixture.Create<long>(), _fixture.Create<CustomersEditRequest>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntityObjectResult>();
        }

        [Fact]
        public async Task Edit_Should_Return_NoContent_When_Command_Result_Is_Successfull()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult();
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Edit.CustomersController(mediatorMock.Object);
            var result = await customerController.Edit(_fixture.Create<long>(), _fixture.Create<CustomersEditRequest>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Edit_Should_Return_Not_Found_When_Command_Result_Is_NotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult(CommandErrorType.NotFound);
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Edit.CustomersController(mediatorMock.Object);
            var result = await customerController.Edit(_fixture.Create<long>(), _fixture.Create<CustomersEditRequest>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_Should_Return_Not_Found_When_Command_Result_Is_NotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult(CommandErrorType.NotFound);
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Delete.CustomersController(mediatorMock.Object);
            var result = await customerController.Delete(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_Should_Return_NoContent_When_Command_Result_Is_Successful()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult();
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Delete.CustomersController(mediatorMock.Object);
            var result = await customerController.Delete(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task Get_Should_Return_OK_With_CustomerDto_Result_When_QueryResult_Has_No_Error()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceQueryResult<CustomerDto>(_fixture.Create<CustomerDto>());
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerGetQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Get.CustomersController(mediatorMock.Object);
            var result = await customerController.Get(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().BeOfType<CustomerDto>();
        }

        [Fact]
        public async Task Get_Should_Return_NotFound_When_QueryResult_Has_NotFound_Error()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceQueryResult<CustomerDto>(null as CustomerDto);
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerGetQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.Get.CustomersController(mediatorMock.Object);
            var result = await customerController.Get(_fixture.Create<long>(), _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();

        }


        [Fact]
        public async Task GetAll_Should_Return_OK_AnyTime()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceQueryResult<IList<CustomerDto>>(_fixture.Create<List<CustomerDto>>());
            mediatorMock.Setup(m => m.Send(It.IsAny<CustomerGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var customerController = new UseCases.Customers.GetAll.CustomersController(mediatorMock.Object);
            var result = await customerController.GetAll(_fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().BeAssignableTo<IList<CustomerDto>>();
        }
    }
}

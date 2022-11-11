using AutoFixture;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Products;
using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Requests.Products;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Domain.Abstraction;




namespace CleanArchitect.Tests.WebControllers
{

    public class ProductsControllerTests
    {
        private Fixture _fixtrue;

        public ProductsControllerTests()
        {
            _fixtrue = new Fixture();
        }

        [Fact]
        public async Task Add_Should_Return_Create_Result_When_CommandResult_Is_Successfull()
        {
            var serviceResult = new ServiceCommandResult(_fixtrue.Create<string>());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var productsController = new UseCases.Products.Add.ProductsController(mediatorMock.Object);
            productsController.Url = GeneralFixtures.CreateMockUrlHelper().Object;
            var result = await productsController.AddProduct(_fixtrue.Create<ProductAddRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();
            ((CreatedResult)result).Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Add_Shoud_Return_UnprocessableEntity_When_CommandResult_Has_Validation_Error()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceResult = new ServiceCommandResult(CommandErrorType.Validation);

            mediatorMock.Setup(m => m.Send(It.IsAny<ProductAddCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(serviceResult);

            var productsController = new UseCases.Products.Add.ProductsController(mediatorMock.Object);
            productsController.Url = GeneralFixtures.CreateMockUrlHelper().Object;

            var result = await productsController.AddProduct(_fixtrue.Create<ProductAddRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntityObjectResult>();

        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_CommandResult_Has_NotFound_Error()
        {
            var commandResutl = new ServiceCommandResult(CommandErrorType.NotFound);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var productsController = new UseCases.Products.Delete.ProductsController(mediatorMock.Object);
            var result = await productsController.Delete(_fixtrue.Create<long>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_should_Return_NoContent_when_CommandResult_Has_No_Error()
        {
            var commandResutl = new ServiceCommandResult();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var productsController = new UseCases.Products.Delete.ProductsController(mediatorMock.Object);
            var result = await productsController.Delete(_fixtrue.Create<long>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task Edit_Should_Return_NotFound_When_CommandResult_Has_NotFound_Error()
        {
            var commandResutl = new ServiceCommandResult(CommandErrorType.NotFound);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var productsController = new UseCases.Products.Edit.ProductsController(mediatorMock.Object);
            var result = await productsController.Edit(_fixtrue.Create<long>(), _fixtrue.Create<ProductEditRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Edit_should_Return_NoContent_when_CommandResult_Has_No_Error()
        {
            var commandResutl = new ServiceCommandResult();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(commandResutl);

            var productsController = new UseCases.Products.Edit.ProductsController(mediatorMock.Object);
            var result = await productsController.Edit(_fixtrue.Create<long>(), _fixtrue.Create<ProductEditRequest>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task Get_Should_Return_NotFound_When_QueryResult_Has_NotFound_Error()
        {
            var queryResutl = new ServiceQueryResult<ProductDto>(null as ProductDto);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductGetQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResutl);

            var productsController = new UseCases.Products.Get.ProductsController(mediatorMock.Object);
            var result = await productsController.Get(_fixtrue.Create<long>(), _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_Should_Return_OK_When_QueryResult_Has_No_Error_With_Same_Id()
        {
            var id = _fixtrue.Create<long>();
            var queryResutl = new ServiceQueryResult<ProductDto>(new ProductDto { Id = id });

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<ProductGetQuery>(p => p.ProductId == id), It.IsAny<CancellationToken>())).ReturnsAsync(queryResutl);

            var productsController = new UseCases.Products.Get.ProductsController(mediatorMock.Object);
            var result = await productsController.Get(id, _fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).Value.Should().BeOfType<ProductDto>();
            ((ProductDto)((OkObjectResult)result).Value).Id.Should().Be(id);
        }

        [Fact]
        public async Task GetList_Should_Return_OK_Always()
        {
            var queryListResult = new ServiceQueryResult<IList<ProductDto>>(new List<ProductDto>());


            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ProductGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryListResult);

            var productsController = new UseCases.Products.GetList.ProductsController(mediatorMock.Object);
            var result = await productsController.GetAll(_fixtrue.Create<CancellationToken>());

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}

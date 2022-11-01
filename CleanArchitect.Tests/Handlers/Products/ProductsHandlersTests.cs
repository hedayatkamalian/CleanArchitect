using AutoFixture;
using AutoMapper;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Products;
using CleanArchitect.Application.UseCases.Products.CommandHandlers;
using CleanArchitect.Application.UseCases.Products.Commands;
using CleanArchitect.Application.UseCases.Products.QueryHandlers;
using CleanArchitect.Domain.Commands.Products;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using FluentAssertions;
using Moq;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Tests.Handlers.Products
{
    public class ProductsHandlersTests
    {
        private readonly Fixture _fixture;

        public ProductsHandlersTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Add_Should_Return_ValidationError_When_Price_Is_Equal_Or_Less_Than_Zero()
        {
            var price = _fixture.Create<decimal>();
            price = price > 0 ? price * -1 : price;
            var command = new ProductAddCommand(_fixture.Create<string>(), price);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new ProductAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await handler.Handle(command, _fixture.Create<CancellationToken>());

            result.ErrorType.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);

        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        public async Task Add_Should_Return_ValidationError_When_Product_Name_Is_Null_Or_Empty(string productName)
        {
            var price = _fixture.Create<decimal>();
            var command = new ProductAddCommand(productName, price);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new ProductAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await handler.Handle(command, _fixture.Create<CancellationToken>());

            result.ErrorType.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);

        }

        [Fact]
        public async Task Add_Should_Return_OK_When_Price_Added_Successfully()
        {
            var price = _fixture.Create<decimal>();
            var command = new ProductAddCommand(_fixture.Create<string>(), price);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


            var handler = new ProductAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await handler.Handle(command, _fixture.Create<CancellationToken>());

            result.WasSuccessfull.Should().BeTrue();
            result.ErrorType.Should().BeNull();
            result.Id.Should().NotBeNull();

        }


        [Fact]
        public async Task Delete_Should_Return_NotFound_Error_When_Id_Does_Not_Exist()
        {
            var productDeleteCommand = new ProductDeleteCommand(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Product);
            unitOfWorkMock.Setup(m => m.ProductRepository.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var productDeleteHandler = new ProductDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await productDeleteHandler.Handle(productDeleteCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.WasSuccessfull.Should().BeFalse();
            result.ErrorType.Should().Be(CommandErrorType.NotFound);

        }

        [Fact]
        public async Task Delete_Should_Return_Done_When_Operation_Is_Successful()
        {
            var productDeleteCommand = new ProductDeleteCommand(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Product>());
            unitOfWorkMock.Setup(m => m.ProductRepository.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var productDeleteHandler = new ProductDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await productDeleteHandler.Handle(productDeleteCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.WasSuccessfull.Should().BeTrue();
            result.ErrorType.Should().BeNull();
        }


        [Fact]
        public async Task Edit_Should_Return_NotFound_Error_When_Id_Does_Not_Exist()
        {
            var productEditCommand = new ProductEditCommand(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<decimal>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Product);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var productEditHandler = new ProductEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await productEditHandler.Handle(productEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.WasSuccessfull.Should().BeFalse();
            result.ErrorType.Should().Be(CommandErrorType.NotFound);

        }

        [Fact]
        public async Task Edit_Should_Return_Done_When_Operation_Is_Successful()
        {
            var productEditCommand = new ProductEditCommand(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<decimal>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Product>());
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var productEditHandler = new ProductEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);

            var result = await productEditHandler.Handle(productEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.WasSuccessfull.Should().BeTrue();
            result.ErrorType.Should().BeNull();
        }


        [Fact]
        public async Task Get_Should_Return_NotFound_Error_When_Id_Does_Not_Exist()
        {
            var productGetQuery = new ProductGetQuery(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Product);

            var product = _fixture.Create<Product>();
            var autoMapper = new Mock<IMapper>();

            var productGetQueryHandler = new ProductGetQueryHandler(unitOfWorkMock.Object, autoMapper.Object);

            var result = await productGetQueryHandler.Handle(productGetQuery, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Result.Should().BeNull();
            result.HasError.Should().BeFalse();
        }

        [Fact]
        public async Task Get_Should_Return_Dto_When_Id_Exist()
        {
            var productGetQuery = new ProductGetQuery(_fixture.Create<long>());

            var product = _fixture.Create<Product>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);

            var autoMapper = new Mock<IMapper>();
            autoMapper.Setup(m => m.Map<ProductDto>(product)).Returns(new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price });

            var productGetQueryHandler = new ProductGetQueryHandler(unitOfWorkMock.Object, autoMapper.Object);

            var result = await productGetQueryHandler.Handle(productGetQuery, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<ProductDto>();
            result.HasError.Should().BeFalse();
        }

        [Fact]
        public async Task GetList_Should_Return_IList_DTO_Anytime()
        {
            var productGetListQuery = new ProductGetListQuery();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<List<Product>>());

            var autoMapper = new Mock<IMapper>();
            autoMapper.Setup(m => m.Map<IList<ProductDto>>(It.IsAny<IList<Product>>())).Returns(_fixture.Create<List<ProductDto>>());

            var productGetQueryHandler = new ProductGetListQueryHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros, autoMapper.Object);

            var result = await productGetQueryHandler.Handle(productGetListQuery, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.HasError.Should().BeFalse();
            result.HasResult.Should().BeTrue();
        }
    }
}

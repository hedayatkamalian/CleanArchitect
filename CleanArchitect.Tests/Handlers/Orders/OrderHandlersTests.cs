using AutoFixture;
using AutoMapper;
using CleanArchitect.Application.Dtos.Orders;
using CleanArchitect.Application.Queries.Orders;
using CleanArchitect.Application.UseCases.Orders.CommandHandlers;
using CleanArchitect.Application.UseCases.Orders.QueryHandlers;
using CleanArchitect.Domain.Commands.Orders;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using CleanArchitect.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using SharedKernel.Domain.Abstraction;
using System.Linq.Expressions;

namespace CleanArchitect.Tests.Handlers
{
    public class OrderHandlersTests
    {
        public Fixture _fixture { get; set; }
        public OrderHandlersTests()
        {
            _fixture = new Fixture();
        }


        [Fact]
        public async Task Add_Should_Return_Validation_Error_When_CustomerId_Does_Not_Exist()
        {
            var orderAddCommand = _fixture.Create<OrderAddCommand>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);

            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Add_Should_Return_Validation_Error_When_AddCommand_Has_No_OrderItem()
        {
            var orderAddCommand = new OrderAddCommand(_fixture.Create<long>(), _fixture.Create<Address>(), new List<OrderItemAddCommand>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Add_Should_Return_Validation_Error_When_Duplicate_OrderItems_Exist_In_Command()
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>().ToList();
            orderItems.Add(orderItems.Last());

            var orderAddCommand = new OrderAddCommand(_fixture.Create<long>(), _fixture.Create<Address>(), orderItems);
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Add_Should_Return_Validation_Error_When_Any_OrderItem_Quantity_Is_Negative_Or_Zero(int quantity)
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>().ToList();
            var badQuantityItem = new OrderItemAddCommand { Quantity = quantity, Discount = _fixture.Create<decimal>(), ProductId = _fixture.Create<long>() };
            orderItems.Add(badQuantityItem);

            var orderAddCommand = new OrderAddCommand(_fixture.Create<long>(), _fixture.Create<Address>(), orderItems);
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Add_Should_Return_Validation_Error_When_AddCommand_Has_OrderItem_With_Wrong_ProductId()
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>().ToList();
            var orderAddCommand = new OrderAddCommand(_fixture.Create<long>(), _fixture.Create<Address>(), orderItems);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.CreateMany<Product>(orderItems.Count - 1).ToList());

            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());

            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Add_Should_Return_Validation_Error_When_OrderItem_Discount_Is_Bigger_Than_Product_Price()
        {
            var orderItem = _fixture.Create<OrderItemAddCommand>();
            var orderAddCommand = new OrderAddCommand(_fixture.Create<long>(), _fixture.Create<Address>(), new List<OrderItemAddCommand> { orderItem });

            var product = new Product { Id = orderItem.ProductId, Name = _fixture.Create<string>(), Price = orderItem.Discount - 1 };
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>() { product });

            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());


            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Add_Should_Return_No_Error_When_Operation_Is_Successfull()
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>();
            foreach (var item in orderItems)
            {
                if (item.Quantity <= 0)
                    item.Quantity = 10;

                if (item.Discount <= 0)
                    item.Discount = 10;
            }


            var products = new List<Product>();
            foreach (var item in orderItems)
            {
                var price = _fixture.Create<Decimal>() * _fixture.Create<int>() + item.Discount;
                products.Add(new Product { Id = item.ProductId, Name = _fixture.Create<string>(), Price = price });
            }

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var customer = Customer.New(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<String>(), _fixture.Create<string>());

            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            unitOfWorkMock.Setup(m => m.OrderRepository.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var orderAddCommand = new OrderAddCommand(customer.Id, _fixture.Create<Address>(), orderItems.ToList());

            var orderAddCommandHandler = new OrderAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderAddCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorMessages.Should().BeNull();
            result.ErrorType.Should().BeNull();
            result.Id.Should().NotBeNull();
            result.Id.Length.Should().BeGreaterThan(0);
            result.WasSuccessfull.Should().BeTrue();

        }






        [Fact]
        public async Task Edit_Should_Return_NotFound_Error_When_OrderId_Does_Not_Exist()
        {
            var orderEditCommand = _fixture.Create<OrderEditCommand>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Order);
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());

            var orderAddCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderAddCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.NotFound);
        }

        [Fact]
        public async Task Edit_Should_Return_Validation_Error_When_CustomerId_Does_Not_Exist()
        {
            var orderEditCommand = _fixture.Create<OrderEditCommand>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Order>());

            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }


        [Fact]
        public async Task Edit_Should_Return_Validation_Error_When_AddCommand_Has_No_OrderItem()
        {
            var orderEditCommand = new OrderEditCommand(_fixture.Create<long>(), _fixture.Create<long>(), _fixture.Create<Address>(), new List<OrderItemAddCommand>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Edit_Should_Return_Validation_Error_When_Duplicate_OrderItems_Exist_In_Command()
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>().ToList();
            orderItems.Add(orderItems.Last());

            var orderEditCommand = new OrderEditCommand(_fixture.Create<long>(), _fixture.Create<long>(), _fixture.Create<Address>(), orderItems);
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);

            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task Edit_Should_Return_Validation_Error_When_Any_OrderItem_Quantity_Is_Negative_Or_Zero(int quantity)
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>().ToList();
            var badQuantityItem = new OrderItemAddCommand { Quantity = quantity, Discount = _fixture.Create<decimal>(), ProductId = _fixture.Create<long>() };
            orderItems.Add(badQuantityItem);

            var orderEditCommand = new OrderEditCommand(_fixture.Create<long>(), _fixture.Create<long>(), _fixture.Create<Address>(), orderItems);
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Edit_Should_Return_Validation_Error_When_EditCommand_Has_OrderItem_With_Wrong_ProductId()
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>().ToList();
            var orderEditCommand = new OrderEditCommand(_fixture.Create<long>(), _fixture.Create<long>(), _fixture.Create<Address>(), orderItems);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.CreateMany<Product>(orderItems.Count - 1).ToList());

            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Order>());

            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Edit_Should_Return_Validation_Error_When_OrderItem_Discount_Is_Bigger_Than_Product_Price()
        {
            var orderItem = _fixture.Create<OrderItemAddCommand>();
            var orderEditCommand = new OrderEditCommand(_fixture.Create<long>(), _fixture.Create<long>(), _fixture.Create<Address>(), new List<OrderItemAddCommand> { orderItem });

            var product = new Product { Id = orderItem.ProductId, Name = _fixture.Create<string>(), Price = orderItem.Discount - 1 };
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>() { product });

            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Order>());


            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }

        [Fact]
        public async Task Edit_Should_Retun_No_Error_When_Operation_Is_Successfull()
        {
            var orderItems = _fixture.CreateMany<OrderItemAddCommand>();
            foreach (var item in orderItems)
            {
                if (item.Quantity <= 0)
                    item.Quantity = 10;

                if (item.Discount <= 0)
                    item.Discount = 10;
            }


            var products = new List<Product>();
            foreach (var item in orderItems)
            {
                var price = _fixture.Create<Decimal>() * _fixture.Create<int>() + item.Discount;
                products.Add(new Product { Id = item.ProductId, Name = _fixture.Create<string>(), Price = price });
            }

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(m => m.ProductRepository.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Create<Order>());

            var customer = Customer.New(_fixture.Create<long>(), _fixture.Create<string>(), _fixture.Create<String>(), _fixture.Create<string>());

            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var orderEditCommand = new OrderEditCommand(_fixture.Create<long>(), customer.Id, _fixture.Create<Address>(), orderItems.ToList());

            var orderEditCommandHandler = new OrderEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderEditCommandHandler.Handle(orderEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorMessages.Should().BeNull();
            result.ErrorType.Should().BeNull();
            result.WasSuccessfull.Should().BeTrue();
        }


        [Fact]
        public async Task Delete_Should_Return_NotFound_Error_When_OrderId_Does_Not_Exist()
        {
            var orderDeleteCommand = _fixture.Create<OrderDeleteCommand>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Order);
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());

            var orderDeleteCommandHandler = new OrderDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderDeleteCommandHandler.Handle(orderDeleteCommand, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.NotFound);
        }

        [Fact]
        public async Task Delete_Should_Return_No_Error_When_Operation_Done()
        {
            var orderDeleteCommand = _fixture.Create<OrderDeleteCommand>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Order>());
            unitOfWorkMock.Setup(m => m.OrderRepository.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var orderDeleteCommandHandler = new OrderDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await orderDeleteCommandHandler.Handle(orderDeleteCommand, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.ErrorType.Should().BeNull();
            result.WasSuccessfull.Should().BeTrue();
        }

        [Fact]
        public async Task Get_Should_Return_Null_When_OrderId_Does_Not_Exist()
        {
            var orderGetQuery = new OrderGetQuery(_fixture.Create<long>());
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Order);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(_fixture.Create<OrderDto>());

            var orderGetQueryHandler = new OrderGetQueryHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros, mapperMock.Object);
            var result = await orderGetQueryHandler.Handle(orderGetQuery, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.Result.Should().BeNull();
        }


        [Fact]
        public async Task Get_Should_Return_OrderDto_When_OrderId_Exist()
        {
            var orderGetQuery = new OrderGetQuery(_fixture.Create<long>());
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Order>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(_fixture.Create<OrderDto>());

            var orderGetQueryHandler = new OrderGetQueryHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros, mapperMock.Object);
            var result = await orderGetQueryHandler.Handle(orderGetQuery, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<OrderDto>();
        }


        [Fact]
        public async Task GetAll_Should_Return_OK_Everytime()
        {
            var orderGetAllQuery = new OrderGetAllQuery();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.OrderRepository.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.CreateMany<Order>().ToList());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IList<OrderDto>>(It.IsAny<IList<Order>>())).Returns(_fixture.CreateMany<OrderDto>().ToList());

            var orderGetAllQueryHandler = new OrderGetAllQueryHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros, mapperMock.Object);
            var result = await orderGetAllQueryHandler.Handle(orderGetAllQuery, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<IList<OrderDto>>();
        }

    }
}

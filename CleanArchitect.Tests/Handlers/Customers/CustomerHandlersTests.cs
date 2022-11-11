using AutoFixture;
using AutoMapper;
using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Queries.Customers;
using CleanArchitect.Application.UseCases.Customers.CommandHandlers;
using CleanArchitect.Application.UseCases.Customers.QueryHandlers;
using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using FluentAssertions;
using Moq;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Tests.Handlers
{
    public class CustomerHandlersTests
    {
        private Fixture _fixture { get { return new Fixture(); } }
        public CustomerHandlersTests()
        {

        }


        [Fact]
        public async Task Add_Should_Return_No_Error_When_Operation_Is_Successfull()
        {
            var customerAddCommand = _fixture.Create<CustomerAddCommand>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()));
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var customerAddCommandHandler = new CustomerAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerAddCommandHandler.Handle(customerAddCommand, _fixture.Create<CancellationToken>());

            result.WasSuccessfull.Should().BeTrue();
            result.ErrorType.Should().BeNull();
            result.Id.Length.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("", "kamal ", "+989150102123")]
        [InlineData("Hed", "  ", "+989150102123")]
        [InlineData("Hed", "kamal", "    ")]
        public async Task Add_Should_Return_Validation_Error_When_FirstName_Or_LastName_Or_PhoneNumber_Are_Empty(string firstName, string lastName, string phoneNumber)
        {
            var customerAddCommand = new CustomerAddCommand(firstName, lastName, phoneNumber);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Verify(m => m.CustomerRepository.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            var customerAddCommandHandler = new CustomerAddCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerAddCommandHandler.Handle(customerAddCommand, _fixture.Create<CancellationToken>());

            result.WasSuccessfull.Should().BeFalse();
            result.ErrorType.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }


        [Fact]
        public async Task Delete_Should_Return_NotFound_Error_When_Id_NotExist()
        {
            var customerDeleteCommand = new CustomerDeleteCommand(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);
            unitOfWorkMock.Verify(m => m.CustomerRepository.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

            var customerDeleteHandler = new CustomerDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerDeleteHandler.Handle(customerDeleteCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.NotFound);

        }

        [Fact]
        public async Task Delete_Should_Return_No_Error_When_Operation_Is_Successfull()
        {
            var customerDeleteCommand = new CustomerDeleteCommand(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var customerDeleteHandler = new CustomerDeleteCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerDeleteHandler.Handle(customerDeleteCommand, _fixture.Create<CancellationToken>());

            unitOfWorkMock.Verify(m => m.CustomerRepository.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().NotBeNull();
            result.ErrorType.Should().BeNull();
            result.WasSuccessfull.Should().BeTrue();

        }

        [Fact]
        public async Task Edit_Should_Return_NotFound_Error_When_Id_NotExist()
        {
            var customerEditCommand = _fixture.Create<CustomerEditCommand>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);

            var customerEditHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerEditHandler.Handle(customerEditCommand, _fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.ErrorType.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.NotFound);

        }

        [Fact]
        public async Task Edit_Should_Return_No_Error_When_Operation_Is_Successfull()
        {
            var customerEditCommand = _fixture.Create<CustomerEditCommand>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var customerEditHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerEditHandler.Handle(customerEditCommand, _fixture.Create<CancellationToken>());

            unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            result.Should().NotBeNull();
            result.ErrorType.Should().BeNull();
            result.WasSuccessfull.Should().BeTrue();

        }

        [Theory]
        [InlineData("", "kamal ", "+989150102123")]
        [InlineData("Hed", "  ", "+989150102123")]
        [InlineData("Hed", "kamal", "    ")]
        public async Task Edit_Should_Return_Validation_Error_When_FirstName_Or_LastName_Or_PhoneNumber_Are_Empty(string firstName, string lastName, string phoneNumber)
        {
            var customerEditCommand = new CustomerEditCommand(_fixture.Create<long>(), firstName, lastName, phoneNumber);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());
            unitOfWorkMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            var customerEditCommandHandler = new CustomerEditCommandHandler(unitOfWorkMock.Object, CommonMocks.ApplicationErros);
            var result = await customerEditCommandHandler.Handle(customerEditCommand, _fixture.Create<CancellationToken>());

            result.WasSuccessfull.Should().BeFalse();
            result.ErrorType.Should().NotBeNull();
            result.ErrorType.Should().Be(CommandErrorType.Validation);
        }


        [Fact]
        public async Task Get_Should_Response_NotFound_Error_When_Id_Does_Not_Exist()
        {
            var customerGetQuery = new CustomerGetQuery(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as Customer);

            var mapperMock = new Mock<IMapper>();

            var customerGetQueryHandler = new CustomerGetQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var result = await customerGetQueryHandler.Handle(customerGetQuery, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.Result.Should().BeNull();

        }

        [Fact]
        public async Task Get_Should_Has_No_Error_When_Id_Exist()
        {
            var customerGetQuery = new CustomerGetQuery(_fixture.Create<long>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<Customer>());

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(_fixture.Create<CustomerDto>());

            var customerGetQueryHandler = new CustomerGetQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var result = await customerGetQueryHandler.Handle(customerGetQuery, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.HasError.Should().BeFalse();

        }

        [Fact]
        public async Task GetList_Should_Return_ILIST_CustomerDto_AnyTime()
        {
            var customerGetAllQuery = new CustomerGetAllQuery();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.Create<List<Customer>>);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IList<CustomerDto>>(It.IsAny<IList<Customer>>())).Returns(_fixture.Create<List<CustomerDto>>());

            var customerGetQueryHandler = new CustomerGetAllQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var result = await customerGetQueryHandler.Handle(customerGetAllQuery, _fixture.Create<CancellationToken>());


            result.Should().NotBeNull();
            result.Result.Should().NotBeNull();
            result.HasError.Should().BeFalse();

        }


    }
}
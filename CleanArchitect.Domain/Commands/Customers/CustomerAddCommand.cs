using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Customers;

public class CustomerAddCommand : IRequest<ServiceCommandResult>
{
    public CustomerAddCommand(string firstName, string lastName, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }

}

using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Customers;

public class CustomerEditCommand : IRequest<ServiceCommandResult>
{
    public CustomerEditCommand(long id, string firstName, string lastName, string phoneNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public long Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }


}


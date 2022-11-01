namespace CleanArchitect.Domain.Entities;

public class Customer
{
    public long Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }

    public static Customer New(long id, string firstName, string lastName, string phoneNumber)
    {
        return new Customer
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber
        };
    }

    public void Edit(string firstName, string lastName, string phoneNumber)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber.Trim();
    }
}

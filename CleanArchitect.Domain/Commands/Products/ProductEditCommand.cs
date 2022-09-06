using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Products;

public class ProductEditCommand : IRequest<ServiceCommandResult>
{
    public ProductEditCommand(long id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public long Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
}

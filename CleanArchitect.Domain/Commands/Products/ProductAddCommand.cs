using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Products;

public class ProductAddCommand : IRequest<ServiceCommandResult>
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public ProductAddCommand(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

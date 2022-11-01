using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect.Domain.Commands.Products;

public class ProductDeleteCommand : IRequest<ServiceCommandResult>
{
    public long Id { get; set; }

    public ProductDeleteCommand(long id)
    {
        Id = id;
    }


}

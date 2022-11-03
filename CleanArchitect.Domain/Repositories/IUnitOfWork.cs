namespace CleanArchitect.Domain.Repositories;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

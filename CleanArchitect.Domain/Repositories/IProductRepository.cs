using CleanArchitect.Domain.Entities;
using System.Linq.Expressions;

namespace CleanArchitect.Domain.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Product?> GetAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken);
    Task<IList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    Task<long> CountAsync(CancellationToken cancellationToken);

}

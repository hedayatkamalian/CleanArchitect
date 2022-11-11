using CleanArchitect.Domain.Entities;
using System.Linq.Expressions;

namespace CleanArchitect.Domain.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Order?> GetAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken);
    Task<IList<Order>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    Task<long> CountAsync(CancellationToken cancellationToken);
    public Task UpdateAsync(Order order);
}

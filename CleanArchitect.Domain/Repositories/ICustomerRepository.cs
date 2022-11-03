using CleanArchitect.Domain.Entities;
using System.Linq.Expressions;

namespace CleanArchitect.Domain.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task<Customer?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Customer?> GetAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken);
    Task<IList<Customer>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<Customer>> GetAllAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    Task<long> CountAsync(CancellationToken cancellationToken);

}

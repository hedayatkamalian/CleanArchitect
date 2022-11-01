using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using CleanArchitect.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitect.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _dataContext;

        public CustomerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _dataContext.Customers.AddAsync(customer, cancellationToken);
        }

        public async Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return await _dataContext.Customers.CountAsync(cancellationToken);
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var customer = await GetAsync(id, cancellationToken);
            _dataContext.Customers.Remove(customer);
        }

        public async Task<IList<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dataContext.Customers.ToListAsync(cancellationToken);
        }

        public async Task<IList<Customer>> GetAllAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dataContext.Customers.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Customer?> GetAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dataContext.Customers.FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}

using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using CleanArchitect.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitect.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _dataContext;

        public OrderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await _dataContext.Orders.AddAsync(order, cancellationToken);
        }

        public async Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return await _dataContext.Orders.CountAsync(cancellationToken);
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var order = await GetAsync(id, cancellationToken);
            _dataContext.Orders.Remove(order);
        }

        public async Task<IList<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dataContext.Orders
                .Include(p => p.Items)
                .Include(p => p.Customer)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dataContext.Orders.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetAsync(long id, CancellationToken cancellationToken)
        {
            return await GetAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Order?> GetAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dataContext.Orders
                .Include(p => p.Items)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(predicate, cancellationToken);

        }

        public Task UpdateAsync(Order order)
        {
            if (_dataContext.Entry(order).State != EntityState.Modified)
            {
                _dataContext.Update(order);

                if (order.Items is not null)
                {
                    foreach (var item in order.Items)
                    {
                        _dataContext.Entry(item).State = EntityState.Added;
                    }
                }

            }
            return Task.CompletedTask;

        }
    }
}

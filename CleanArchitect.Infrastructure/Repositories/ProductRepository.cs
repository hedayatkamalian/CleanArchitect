using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.Repositories;
using CleanArchitect.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitect.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dataContext;

    public ProductRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _dataContext.Products.AddAsync(product);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await _dataContext.Products.CountAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var product = await GetAsync(id, cancellationToken);
        _dataContext.Products.Remove(product);
    }

    public async Task<IList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dataContext.Products.ToListAsync();
    }

    public async Task<IList<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dataContext.Products.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetAsync(long id, CancellationToken cancellationToken)
    {

        return await GetAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dataContext.Products.FirstOrDefaultAsync(predicate, cancellationToken);
    }


}

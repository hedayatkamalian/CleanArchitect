using CleanArchitect.Domain.Repositories;
using CleanArchitect.Infrastructure.EFCore;

namespace CleanArchitect.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private IProductRepository _productRepository;

    public UnitOfWork(DataContext context)
    {
        _context = context;

    }
    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}

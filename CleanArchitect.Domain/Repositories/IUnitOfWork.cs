﻿namespace CleanArchitect.Domain.Repositories;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IOrderRepository OrderRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Customers;
using CleanArchitect.Application.Queries.Products;
using CleanArchitect.Application.UseCases.Customers.CommandHandlers;
using CleanArchitect.Application.UseCases.Customers.QueryHandlers;
using CleanArchitect.Application.UseCases.Products.CommandHandlers;
using CleanArchitect.Application.UseCases.Products.Commands;
using CleanArchitect.Application.UseCases.Products.QueryHandlers;
using CleanArchitect.Domain.Commands.Customers;
using CleanArchitect.Domain.Commands.Products;
using MediatR;
using SharedKernel.Domain.Abstraction;

namespace CleanArchitect
{
    public static class HandlerRegisterer
    {
        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<ProductAddCommand, ServiceCommandResult>, ProductAddCommandHandler>();
            services.AddScoped<IRequestHandler<ProductEditCommand, ServiceCommandResult>, ProductEditCommandHandler>();
            services.AddScoped<IRequestHandler<ProductDeleteCommand, ServiceCommandResult>, ProductDeleteCommandHandler>();
            services.AddScoped<IRequestHandler<ProductGetQuery, ServiceQueryResult<ProductDto>>, ProductGetQueryHandler>();
            services.AddScoped<IRequestHandler<ProductGetAllQuery, ServiceQueryResult<IList<ProductDto>>>, ProductGetAllQueryHandler>();

            services.AddScoped<IRequestHandler<CustomerAddCommand, ServiceCommandResult>, CustomerAddCommandHandler>();
            services.AddScoped<IRequestHandler<CustomerEditCommand, ServiceCommandResult>, CustomerEditCommandHandler>();
            services.AddScoped<IRequestHandler<CustomerDeleteCommand, ServiceCommandResult>, CustomerDeleteCommandHandler>();
            services.AddScoped<IRequestHandler<CustomerGetQuery, ServiceQueryResult<CustomerDto>>, CustomerGetQueryHandler>();
            services.AddScoped<IRequestHandler<CustomerGetAllQuery, ServiceQueryResult<IList<CustomerDto>>>, CustomerGetAllQueryHandler>();


            return services;
        }
    }
}

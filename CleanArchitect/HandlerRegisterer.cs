using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Application.Queries.Products;
using CleanArchitect.Application.UseCases.Products.CommandHandlers;
using CleanArchitect.Application.UseCases.Products.Commands;
using CleanArchitect.Application.UseCases.Products.QueryHandlers;
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
            services.AddScoped<IRequestHandler<ProductGetListQuery, ServiceQueryResult<IList<ProductDto>>>, ProductGetListQueryHandler>();

            return services;
        }
    }
}

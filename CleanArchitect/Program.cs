using CleanArchitect;
using CleanArchitect.Application.Options;
using CleanArchitect.Domain.Repositories;
using CleanArchitect.Infrastructure.EFCore;
using CleanArchitect.Infrastructure.Repositories;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(Program));


var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False");
connection.Open();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connection));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<ApplicationErrors>(builder.Configuration.GetSection(nameof(ApplicationErrors)));
builder.Services.RegisterHandlers();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using CleanArchitect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitect.Infrastructure.EFCore
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {

        }

        public DbSet<Job> Jobs { get; set; }
    }
}

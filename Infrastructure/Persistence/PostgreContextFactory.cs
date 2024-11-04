using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence
{
    public class PostgreDataContextFactory : IDesignTimeDbContextFactory<PostgreDataContext>
    {
        public PostgreDataContext CreateDbContext(string[] args)
        {
            string? connectionString = args.FirstOrDefault();

            var optionsBuilder = new DbContextOptionsBuilder<PostgreDataContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreDataContext(optionsBuilder.Options);
        }
    }
}

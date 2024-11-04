using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence
{
    public class SqlDataContextFactory : IDesignTimeDbContextFactory<SqlDataContext>
    {
        public SqlDataContext CreateDbContext(string[] args)
        {
            string? connectionString = args.FirstOrDefault();

            var optionsBuilder = new DbContextOptionsBuilder<SqlDataContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new SqlDataContext(optionsBuilder.Options);
        }
    }
}

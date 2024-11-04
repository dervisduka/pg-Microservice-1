using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextInitialiser
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger;
        private readonly SqlDataContext _sqlDataContext;
        private readonly PostgreDataContext _postgreDataContext;
        private bool _useSqlServer;
        public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, SqlDataContext sqlDataContext, PostgreDataContext PostgreDataContext, IConfiguration configuration)
        {
            _logger = logger;
            _sqlDataContext = sqlDataContext;
            _postgreDataContext = PostgreDataContext;
            _useSqlServer = bool.TryParse(configuration["UseSqlServer"], out bool result) && result;
        }

        private DbContext _context => _useSqlServer ? _sqlDataContext : _postgreDataContext;
        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
    }
}

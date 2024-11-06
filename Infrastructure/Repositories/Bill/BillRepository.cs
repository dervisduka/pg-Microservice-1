using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Entities.Bills.Queries;
using Dapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<BillRepository> _logger;
        private DbSession _session;
        private readonly ICurrentActorService _currentUserService;

        public BillRepository(IConfiguration configuration, DbSession session, ILogger<BillRepository> logger, ICurrentActorService currentUserService)
        {
            this.configuration = configuration;
            this._session = session;
            this._logger = logger;
            this._currentUserService = currentUserService;
        }
        public async Task<Bill> AddAsync(Bill entity)
        {
            entity.CreatedDate = DateTime.Now;
            var sql = "INSERT INTO Bills (Amount, Status, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy) " +
              "VALUES (@Amount, @Status, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy); " +
              "SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleAsync<int>(sql, entity);
                entity.Id = result;
                return entity;
            }

        }
        public async Task<Bill> AddAsyncTransactional(Bill entity)
        {
            entity.CreatedDate = DateTime.Now;

            var sql = "INSERT INTO Bills (Amount, Status, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy) " +
                      "VALUES (@Amount, @Status, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy); " +
                      "SELECT CAST(SCOPE_IDENTITY() as int)";

            // Execute the query and retrieve the new Id
            var result = await _session.Connection.QuerySingleAsync<int>(sql, entity, _session.Transaction);
            entity.Id = result;

            return entity;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Bills WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }
        public async Task<IReadOnlyList<Bill>> GetAllAsync()
        {
            var sql = "SELECT * FROM Bills";
            _logger.LogInformation("[BillsRepo] hyri ne data layer 1 me query {@sql}", sql);
            _logger.LogInformation($"[BillsRepo] hyri ne data layer 2 me query {sql}");
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Bill>(sql);
                return result.ToList();
            }
        }
        public async Task<Bill> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Bills WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Bill>(sql, new { Id = id });
                return result;
            }
        }
        public async Task<Bill?> UpdateAsync(Bill entity)
        {
            entity.ModifiedDate = DateTime.Now;
            var sql = "UPDATE Bills SET Amount = @Amount, Status = @Status, CreatedDate = @CreatedDate, CreatedBy = @CreatedBy, ModifiedDate = @ModifiedDate,ModifiedBy=@ModifiedBy   WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                if (result > 0)
                    return entity;
                else
                    return null;
            }
        }

        public async Task<PaginatedList<Bill>> GetBillsByCriteria(BillsWithPaginationCriteria criteria)
        {
            List<Bill> Items;
            int TotalCount;
            string whereStatement = BuildBillWhereStatement(criteria);

            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = @"SELECT [Amount], [Status], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy] 
            FROM [dbo].[Bills] " + whereStatement +
                @" ORDER BY [Id] DESC
            OFFSET (@pageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*) 
            FROM [dbo].[Bills] " + whereStatement;

                await connection.OpenAsync();
                using (var multi = await connection.QueryMultipleAsync(query, criteria))
                {
                    Items = multi.Read<Bill>().ToList();
                    TotalCount = multi.ReadFirst<int>();
                }
            }


            return new PaginatedList<Bill>(Items, TotalCount, criteria.PageNumber, criteria.PageSize);
        }

        private string BuildBillWhereStatement(BillsWithPaginationCriteria criteria)
        {
            string _returnValue = @"WHERE ";
            if (criteria.Id != null)
                _returnValue += " Id = @Id AND";

            if (criteria.IsCreatedDateBetweenUsed)
            {
                _returnValue += " [CreatedDate] between @@CreatedDateFrom and @CreatedDateTo AND";
            }

            if (criteria.Amount != null)
                _returnValue += " Amount = @Amount AND";

            if (criteria.Status != null)
                _returnValue += " Status = @Status AND";

            if (criteria.Id != null)
                _returnValue += " Id = @Id AND";

            if (_returnValue == "WHERE ")
                return "";
            return _returnValue.Substring(0, _returnValue.Length - 4);
        }
    }
}
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Entities.Products.Queries;
using Dapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ProductRepository> _logger;
        private DbSession _session;
        private readonly ICurrentActorService _currentUserService;

        public ProductRepository(IConfiguration configuration, DbSession session, ILogger<ProductRepository> logger, ICurrentActorService currentUserService)
        {
            this.configuration = configuration;
            this._session = session;
            this._logger = logger;
            this._currentUserService = currentUserService;
        }
        public async Task<Product> AddAsync(Product entity)
        {
            entity.CreatedDate = DateTime.Now;
            var sql = "Insert into Products (Name,Description,Barcode,Rate,AddedOn) VALUES (@Name,@Description,@Barcode,@Rate,@AddedOn);" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleAsync<int>(sql, entity);
                entity.Id = result;
                return entity;
            }

        }

        public async Task<Product> AddAsyncTransactional(Product entity)
        {

            entity.CreatedDate = DateTime.Now;
            var sql = "Insert into Products (Name,Description,Barcode,Rate,CreatedDate,CreatedBy) VALUES (@Name,@Description,@Barcode,@Rate,@CreatedDate,@CreatedBy);" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";


            var result = await _session.Connection.QuerySingleAsync<int>(sql, entity, _session.Transaction);
            entity.Id = result;
            return entity;

        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }
        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products";
            _logger.LogInformation("[ProductsRepo] hyri ne data layer 1 me query {@sql}", sql);
            _logger.LogInformation($"[ProductsRepo] hyri ne data layer 2 me query {sql}");
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Product>(sql);
                return result.ToList();
            }
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
                return result;
            }
        }
        public async Task<Product?> UpdateAsync(Product entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedBy = Int32.Parse(_currentUserService.UserId);
            var sql = "UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, Rate = @Rate, ModifiedDate = @ModifiedDate,ModifiedBy=@ModifiedBy   WHERE Id = @Id and timestamp=@timestamp";
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

        public async Task<PaginatedList<Product>> GetProductsByCriteria(ProductsWithPaginationCriteria criteria)
        {
            List<Product> Items;
            int TotalCount;
            string whereStatement = BuildProductWhereStatement(criteria);

            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = @"SELECT [Id],[Name],[Barcode],[Description],[Rate],[CreatedDate],[ModifiedDate],[timestamp] 
            FROM [dbo].[Products] " + whereStatement +
                @" ORDER BY [Id] DESC
            OFFSET (@pageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(*) 
            FROM [dbo].[Products] " + whereStatement;

                await connection.OpenAsync();
                using (var multi = await connection.QueryMultipleAsync(query, criteria))
                {
                    Items = multi.Read<Product>().ToList();
                    TotalCount = multi.ReadFirst<int>();
                }
            }


            return new PaginatedList<Product>(Items, TotalCount, criteria.PageNumber, criteria.PageSize);
        }

        private string BuildProductWhereStatement(ProductsWithPaginationCriteria criteria)
        {
            string _returnValue = @"WHERE ";
            if (criteria.Id != null)
                _returnValue += " Id = @Id AND";

            if (criteria.UseNameWithLike)
            {
                _returnValue += " Name like  CONCAT('%',@Name,'%') AND";
            }
            else
            {
                if (criteria.Name != null)
                    _returnValue += " Name = @Name AND";
            }


            if (criteria.Description != null)
                _returnValue += " Description = @Description AND";

            if (criteria.Barcode != null)
                _returnValue += " Barcode = @Barcode AND";

            if (criteria.Rate != null)
                _returnValue += " Rate = @Rate AND";

            if (criteria.IsCreatedDateBetweenUsed)
            {
                _returnValue += " [CreatedDate] between @@CreatedDateFrom and @CreatedDateTo AND";
            }
            else
            {
                if (criteria.CreatedDateFrom != null)
                    _returnValue += " CAST([CreatedDate] as date) = cast(@CreatedDateFrom as date) AND";
            }

            if (_returnValue == "WHERE ")
                return "";
            return _returnValue.Substring(0, _returnValue.Length - 4);
        }
    }
}
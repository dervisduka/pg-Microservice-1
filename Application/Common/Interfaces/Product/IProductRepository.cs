using Application.Common.Models;
using Application.Entities.Products.Queries;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> AddAsyncTransactional(Product entity);
        Task<PaginatedList<Product>> GetProductsByCriteria(ProductsWithPaginationCriteria criteria);
    }
}

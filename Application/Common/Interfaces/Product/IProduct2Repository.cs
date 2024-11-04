using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IProduct2Repository : IGenericRepository<Product>
    {
        Task<Product> AddAsyncTransactional(Product entity);
    }
}

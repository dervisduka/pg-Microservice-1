using Application.Common.Models;
using Application.Entities.Orders.Queries;
using Application.Entities.Products.Queries;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<PaginatedList<Order>> GetOrdersByCriteria(OrdersWithPaginationCriteria criteria);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

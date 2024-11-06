using Application.Common.Models;
using Application.Entities.Bills.Queries;
using Application.Entities.Orders.Queries;
using Application.Entities.Products.Queries;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        Task<PaginatedList<Bill>> GetBillsByCriteria(BillsWithPaginationCriteria criteria);

        Task<Bill> AddAsyncTransactional(Bill entity);

    }
}

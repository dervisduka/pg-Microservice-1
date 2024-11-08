using Application.Common.Models;
using Application.Entities.Alcohols.Queries;
using Application.Entities.Products.Queries;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IAlcoholRepository : IGenericRepository<Alcohol>
    {
        Task<PaginatedList<Alcohol>> GetAlcoholsByCriteria(AlcoholsWithPaginationCriteria criteria);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

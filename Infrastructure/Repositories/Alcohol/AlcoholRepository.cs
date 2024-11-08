using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Entities.Alcohols.Queries;
using Application.Entities.Products.Queries;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Infrastructure.Repositories
{
    public class AlcoholRepository : IAlcoholRepository
    {
        private readonly SqlDataContext _sqlDataContext;
        private readonly PostgreDataContext _PostgreDataContext;
        private readonly bool _useSqlServer;

        public AlcoholRepository(SqlDataContext sqlDataContext, PostgreDataContext PostgreDataContext, IConfiguration configuration)
        {
            _sqlDataContext = sqlDataContext;
            _PostgreDataContext = PostgreDataContext;

            _useSqlServer = bool.TryParse(configuration["UseSqlServer"], out bool result) && result;
        }

        private DbContext CurrentContext => _useSqlServer ? _sqlDataContext : _PostgreDataContext;
        public async Task<PaginatedList<Alcohol>> GetAlcoholsByCriteria(AlcoholsWithPaginationCriteria criteria)
        {
            var query = CurrentContext.Set<Alcohol>().AsQueryable();
            BuildWhereStatement(criteria, ref query);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new PaginatedList<Alcohol>(items, totalCount, criteria.PageNumber, criteria.PageSize);
        }

        public void BuildWhereStatement(AlcoholsWithPaginationCriteria criteria, ref IQueryable<Alcohol> query)
        {
            if (criteria.Id != null)
            {
                query = query.Where(p => p.Id == criteria.Id);
            }

            if (criteria.Amount != null)
            {
                query = query.Where(p => p.Amount == criteria.Amount);
            }

            if (criteria.PricePerBottle != null) { 
            
                query = query.Where(p => p.PricePerBottle == criteria.PricePerBottle);
            }


            if (criteria.Status != null)
            {

                query = query.Where(p => p.Status == criteria.Status);
            }


            if (criteria.AlcoholPercentage != null)
            {

                query = query.Where(p => p.AlcoholPercentage == criteria.AlcoholPercentage);
            }

        }

        public async Task<Alcohol> GetByIdAsync(int id)
        {
            return await CurrentContext.Set<Alcohol>().FindAsync(id);
        }

        public async Task<IReadOnlyList<Alcohol>> GetAllAsync()
        {
            try
            {
                return await CurrentContext.Set<Alcohol>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Alcohol> AddAsync(Alcohol entity)
        {
            CurrentContext.Set<Alcohol>().Add(entity);
            return entity;
        }

        public async Task<Alcohol> UpdateAsync(Alcohol entity)
        {
            try
            {
                CurrentContext.Set<Alcohol>().Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await CurrentContext.Set<Alcohol>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                CurrentContext.Set<Alcohol>().Remove(entity);
            }

            return 0;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await CurrentContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}

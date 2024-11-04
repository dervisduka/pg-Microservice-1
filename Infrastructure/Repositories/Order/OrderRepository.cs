using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Entities.Orders.Queries;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly SqlDataContext _sqlDataContext;
        private readonly PostgreDataContext _PostgreDataContext;
        private readonly bool _useSqlServer;

        public OrderRepository(SqlDataContext sqlDataContext, PostgreDataContext PostgreDataContext, IConfiguration configuration)
        {
            _sqlDataContext = sqlDataContext;
            _PostgreDataContext = PostgreDataContext;

            _useSqlServer = bool.TryParse(configuration["UseSqlServer"], out bool result) && result;
        }

        private DbContext CurrentContext => _useSqlServer ? _sqlDataContext : _PostgreDataContext;
        public async Task<PaginatedList<Order>> GetOrdersByCriteria(OrdersWithPaginationCriteria criteria)
        {
            var query = CurrentContext.Set<Order>().AsQueryable();
            BuildWhereStatement(criteria, ref query);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new PaginatedList<Order>(items, totalCount, criteria.PageNumber, criteria.PageSize);
        }

        public void BuildWhereStatement(OrdersWithPaginationCriteria criteria, ref IQueryable<Order> query)
        {
            if (criteria.Id != null)
            {
                query = query.Where(p => p.Id == criteria.Id);
            }

            if (criteria.UseAddressWithLike)
            {
                query = query.Where(p => p.Address.Contains(criteria.Address));
            }
            else if (criteria.Address != null)
            {
                query = query.Where(p => p.Address == criteria.Address);
            }

            if (criteria.IsCreatedDateBetweenUsed)
            {
                query = query.Where(p => p.CreatedDate >= criteria.CreatedDateFrom && p.CreatedDate <= criteria.CreatedDateTo);
            }
            else if (criteria.CreatedDateFrom != null)
            {
                query = query.Where(p => EF.Functions.DateDiffDay(p.CreatedDate, criteria.CreatedDateFrom) == 0);
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await CurrentContext.Set<Order>().FindAsync(id);
        }

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            try
            {
                return await CurrentContext.Set<Order>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Order> AddAsync(Order entity)
        {
            CurrentContext.Set<Order>().Add(entity);
            return entity;
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            try
            {
                CurrentContext.Set<Order>().Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await CurrentContext.Set<Order>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                CurrentContext.Set<Order>().Remove(entity);
            }

            return 0;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await CurrentContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}

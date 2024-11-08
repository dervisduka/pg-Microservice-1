using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace Infrastructure.Persistence
{
    public class SqlDataContext : DbContext
    {
        private readonly IMediator _mediator;
        public SqlDataContext(DbContextOptions<SqlDataContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Alcohol> Alcohols { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Order>()
           .Property(p => p.timestamp)
           .IsRowVersion()
           .IsRequired(false);

            modelBuilder.Entity<Alcohol>()
            .Property(p => p.timestamp)
           .IsRowVersion()
           .IsRequired(false);

        }
    }
}

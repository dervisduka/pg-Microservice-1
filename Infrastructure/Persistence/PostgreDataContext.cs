using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;


namespace Infrastructure.Persistence
{
    public class PostgreDataContext : DbContext
    {
        private readonly IMediator _mediator;

        public PostgreDataContext(DbContextOptions<PostgreDataContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(p => p.CreatedDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Order>()
            .Property<uint>("xmin")  // Define a shadow property for xmin
            .IsRowVersion();


            modelBuilder.Entity<Order>()
           .Property(p => p.timestamp)
           .IsRowVersion()
           .IsRequired(false);

        }

    }
}

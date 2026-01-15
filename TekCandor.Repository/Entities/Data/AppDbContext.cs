using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekCandor.Repository.Entities.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cycle> Cycles { get; set; }

        public DbSet<Hub> Hub { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<ReturnReason> ReturnReason { get; set; }
        public DbSet<ClearingStatuses> ClearingStatuses { get; set; }
        public DbSet<Users>Users { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cycle>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Code).IsRequired().HasMaxLength(10);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}

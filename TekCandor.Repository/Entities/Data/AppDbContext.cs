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
        public DbSet<User>Users { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        
        public DbSet<Group> Group { get; set; }
        public DbSet<Permission> Permission { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cycle>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Code).IsRequired().HasMaxLength(10);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Hub>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.Id)
                      .ValueGeneratedOnAdd(); 

                entity.Property(h => h.Code)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(h => h.Name)
                      .HasMaxLength(256)
                      .IsUnicode(false);

                entity.Property(h => h.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(h => h.UpdatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(h => h.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(h => h.CrAccSameDay)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(h => h.CrAccNormal)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(h => h.CrAccIntercity)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(h => h.CrAccDollar)
                      .HasMaxLength(128)
                      .IsUnicode(false);
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id)
                      .ValueGeneratedOnAdd(); 

                entity.Property(b => b.Code)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(b => b.NIFTBranchCode)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(b => b.Name)
                      .HasMaxLength(256)
                      .IsUnicode(false);

                entity.Property(b => b.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(b => b.UpdatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(b => b.IsDeleted)
                      .HasDefaultValue(false);

                

                entity.Property(b => b.Email1).HasMaxLength(128).IsUnicode(false);
                entity.Property(b => b.Email2).HasMaxLength(128).IsUnicode(false);
                entity.Property(b => b.Email3).HasMaxLength(128).IsUnicode(false);

               
                entity.HasOne(b => b.Hub)
                      .WithMany()       
                      .HasForeignKey(b => b.HubId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });
           
            modelBuilder.Entity<ReturnReason>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                      .ValueGeneratedOnAdd(); 

                entity.Property(r => r.Code)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(r => r.AlphaReturnCodes)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(r => r.NumericReturnCodes)
                      .HasMaxLength(50)
                      .IsUnicode(false)
                      .IsRequired(); 

                entity.Property(r => r.DescriptionWithReturnCodes)
                      .HasMaxLength(256)
                      .IsUnicode(false);

                entity.Property(r => r.Name)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(r => r.DefaultCallBack)
                      .HasDefaultValue(false);

                entity.Property(r => r.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(r => r.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(r => r.UpdatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(r => r.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(r => r.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd(); 

                entity.Property(u => u.Name)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(u => u.LoginName)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(u => u.PasswordHash)
                      .HasMaxLength(256)
                      .IsUnicode(false);

                entity.Property(u => u.Email)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(u => u.BranchorHub)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(u => u.UserType)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(u => u.PhoneNo)
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.Property(u => u.PasswordLastChanged)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(u => u.IsActive)
                      .HasDefaultValue(true);

                entity.Property(u => u.IsSupervise)
                      .HasDefaultValue(false);

                entity.Property(u => u.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(u => u.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(u => u.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(u => u.UpdatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(u => u.Hub)
                      .WithMany()
                      .HasForeignKey(u => u.HubIds)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Branch)
                      .WithMany()
                      .HasForeignKey(u => u.BranchIds)
                      .OnDelete(DeleteBehavior.Restrict);
            });



        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Models;

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

        public DbSet<ApplicationConfig> ApplicationConfig { get; set; }
        public DbSet<HostCall> HostCall { get; set; }
        public DbSet<HostCallConfig> HostCallConfig { get; set; }
        public DbSet<ImportData> ImportData { get; set; }
        public DbSet<ImportDataDetail> ImportDataDetail { get; set; }
        public DbSet<Manual_ImportData> Manual_ImportData { get; set; }
        public DbSet<Manual_ImportDataDetails> Manual_ImportDataDetails { get; set; }

        public DbSet<SecurityGroup_PermissionRecord> SecurityGroup_PermissionRecord { get; set; }
        public DbSet<Setting> Setting { get; set; }

        public DbSet<ChequedepositInfo> chequedepositInformation { get; set; }

        public DbSet<Bank> Bank { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Instruments> Instruments { get; set; }
        public DbSet<PostingRestriction> PostingRestriction { get; set; }
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
                      .IsRequired(false);

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
                      .IsRequired(false);

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
                      .IsRequired(false);

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

               

                entity.Property(u => u.PhoneNo)
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.Property(u => u.PasswordLastChanged)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(u => u.IsActive)
                      .HasDefaultValue(true);

               

                entity.Property(u => u.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(u => u.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(u => u.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(u => u.UpdatedOn)
                      .IsRequired(false);

               
            });
            modelBuilder.Entity<ApplicationConfig>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()                 
                      .ValueGeneratedOnAdd();      

                entity.Property(e => e.Key)
                      .HasMaxLength(256)
                      .IsRequired(false);           

                entity.Property(e => e.Value)
                      .HasMaxLength(500)
                      .IsRequired(false);           

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);           

                entity.Property(e => e.IsDeleted)
                      .IsRequired()               
                      .HasDefaultValue(false);     

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);           

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);           

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);           

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);           
            });
            modelBuilder.Entity<HostCall>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()          
                      .ValueGeneratedOnAdd();      

                entity.Property(e => e.ChequeDeposit_Id)
                      .IsRequired();                

                entity.Property(e => e.RequsetDate)
                      .IsRequired();             

                entity.Property(e => e.ResponseDate)
                      .IsRequired();               

                entity.Property(e => e.URL)
                      .HasMaxLength(500)
                      .IsRequired(false);              

                entity.Property(e => e.RequestMsg)
                      .IsRequired(false);              

                entity.Property(e => e.ResponseMsg)
                      .IsRequired(false);              

                entity.Property(e => e.ResponseCode)
                      .HasMaxLength(50)
                      .IsRequired(false);              

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);              

                entity.Property(e => e.IsApproved)
                      .IsRequired();                   

                entity.Property(e => e.IsDeleted)
                      .IsRequired()               
                      .HasDefaultValue(false);        

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);              

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);              

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);              

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);              
            });
            modelBuilder.Entity<HostCallConfig>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()               
                      .ValueGeneratedOnAdd();      

                entity.Property(e => e.ParamName)
                      .HasMaxLength(256)
                      .IsRequired(false);             

                entity.Property(e => e.ParamValue)
                      .HasMaxLength(500)
                      .IsRequired(false);             

                entity.Property(e => e.DataType)
                      .HasMaxLength(100)
                      .IsRequired(false);             

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);             

                entity.Property(e => e.IsDeleted)
                      .IsRequired()              
                      .HasDefaultValue(false);      

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);             

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);             

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);             

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);             
            });
            modelBuilder.Entity<ImportData>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()        
                      .ValueGeneratedOnAdd();       

                entity.Property(e => e.FileName)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.Date)
                      .IsRequired();             

                entity.Property(e => e.TotalRecords)
                      .IsRequired();          

                entity.Property(e => e.SuccessfullRecords)
                      .IsRequired();                

                entity.Property(e => e.FailureRecords)
                      .IsRequired();               

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.IsDeleted)
                      .IsRequired()                 
                      .HasDefaultValue(false);     

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);            
            });
            modelBuilder.Entity<ImportDataDetail>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()                
                      .ValueGeneratedOnAdd();   

                entity.Property(e => e.Data)
                      .HasMaxLength(500)
                      .IsRequired(false);            

                entity.Property(e => e.Date)
                      .IsRequired();             

                entity.Property(e => e.Error)
                      .IsRequired();               

                entity.Property(e => e.ErrorDescription)
                      .HasMaxLength(500)
                      .IsRequired(false);            

                entity.Property(e => e.ImportDataId)
                      .IsRequired();            

                entity.HasOne(e => e.ImportData)
                      .WithMany()                     
                      .HasForeignKey(e => e.ImportDataId)
                      .OnDelete(DeleteBehavior.Restrict); 

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.IsDeleted)
                      .IsRequired()                  
                      .HasDefaultValue(false);       

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);            
            });
            modelBuilder.Entity<Manual_ImportData>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()              
                      .ValueGeneratedOnAdd();    

                entity.Property(e => e.FileName)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.Date)
                      .IsRequired();               

                entity.Property(e => e.TotalRecords)
                      .IsRequired();            

                entity.Property(e => e.SuccessfullRecords)
                      .IsRequired();           

                entity.Property(e => e.FailureRecords)
                      .IsRequired();                

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.IsDeleted)
                      .IsRequired()                 
                      .HasDefaultValue(false);      

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);            
            });
            modelBuilder.Entity<Manual_ImportDataDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()          
                      .ValueGeneratedOnAdd();  

                entity.Property(e => e.Data)
                      .HasMaxLength(500)
                      .IsRequired(false);            

                entity.Property(e => e.Date)
                      .IsRequired();                 

                entity.Property(e => e.Error)
                      .IsRequired();                 

                entity.Property(e => e.ErrorDescription)
                      .HasMaxLength(500)
                      .IsRequired(false);            

                entity.Property(e => e.Manual_ImportDataId)
                      .IsRequired();                 

                entity.HasOne(e => e.Manual_ImportData)
                      .WithMany()                     
                      .HasForeignKey(e => e.Manual_ImportDataId)
                      .OnDelete(DeleteBehavior.Restrict); 

                entity.Property(e => e.IsDeleted)
                      .IsRequired()                 
                      .HasDefaultValue(false);    

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);            
            });
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()                  
                      .ValueGeneratedOnAdd();        

                entity.Property(e => e.Description)
                      .HasMaxLength(500)
                      .IsRequired(false);            

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.IsDeleted)
                      .IsRequired()              
                      .HasDefaultValue(false);       

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);            
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .IsRequired()                
                      .ValueGeneratedOnAdd();       

                entity.Property(e => e.Name)
                      .HasMaxLength(256)
                      .IsRequired(false);            

                entity.Property(e => e.Description)
                      .HasMaxLength(500)
                      .IsRequired(false);            

                entity.Property(e => e.IsDeleted)
                      .IsRequired()                  
                      .HasDefaultValue(false);       

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);            

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(128)
                      .IsRequired(false);         

                entity.Property(e => e.CreatedOn)
                      .IsRequired(false);        

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);           
            });
            modelBuilder.Entity<SecurityGroup_PermissionRecord>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.PermissionId }); 

                entity.Property(e => e.GroupId).IsRequired();
                entity.Property(e => e.PermissionId).IsRequired();

              

                // Foreign keys
                entity.HasOne<Group>()
                      .WithMany()
                      .HasForeignKey(e => e.GroupId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Permission>()
                      .WithMany()
                      .HasForeignKey(e => e.PermissionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.Value)
                      .HasMaxLength(2048)
                      .IsUnicode(false);

                entity.Property(u => u.Name)
                      .HasMaxLength(256)
                      .IsUnicode(false);

                entity.Property(e => e.IsDeleted)
                      .IsRequired()
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
                      .IsRequired(false);


            });
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id)
                      .ValueGeneratedOnAdd();   

                entity.Property(b => b.Code)
                      .HasMaxLength(256)
                      .IsUnicode(true);   

                entity.Property(b => b.Name)
                      .HasMaxLength(256)
                      .IsUnicode(true);   

                entity.Property(b => b.Version)
                      .IsRequired();

                entity.Property(b => b.IsNew)
                      .HasDefaultValue(false);

                entity.Property(b => b.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(b => b.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(b => b.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(b => b.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(b => b.UpdatedOn)
                      .IsRequired(false);
            });
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Id)
                      .ValueGeneratedOnAdd();   

                entity.Property(c => c.CurrencyCode)
                      .HasMaxLength(64)
                      .IsUnicode(true);   

                entity.Property(c => c.Rate)
                      .HasColumnType("decimal(19,4)")
                      .IsRequired();

                entity.Property(c => c.DisplayLocale)
                      .HasMaxLength(64)
                      .IsUnicode(true);

                entity.Property(c => c.CustomFormatting)
                      .HasMaxLength(64)
                      .IsUnicode(true);

                entity.Property(c => c.Description)
                      .HasMaxLength(128)
                      .IsUnicode(true);

                entity.Property(c => c.Name)
                      .HasMaxLength(256)
                      .IsUnicode(true);

                entity.Property(c => c.Version)
                      .IsRequired();

                entity.Property(c => c.DisplayOrder)
                      .IsRequired();

                entity.Property(c => c.Published)
                      .HasDefaultValue(false);

                entity.Property(c => c.IsNew)
                      .HasDefaultValue(false);

                entity.Property(c => c.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(c => c.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(c => c.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(c => c.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(c => c.UpdatedOn)
                      .IsRequired(false);

            });
            modelBuilder.Entity<Instruments>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Id)
                      .ValueGeneratedOnAdd();   

                entity.Property(i => i.Code)
                      .HasMaxLength(128)
                      .IsUnicode(false);   

                entity.Property(i => i.Name)
                      .HasMaxLength(256)
                      .IsUnicode(true);   

                entity.Property(i => i.Version)
                      .IsRequired();

                entity.Property(i => i.IsNew)
                      .HasDefaultValue(false);

                entity.Property(i => i.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(i => i.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(i => i.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(i => i.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(i => i.UpdatedOn)
                      .IsRequired(false);

            });
            modelBuilder.Entity<PostingRestriction>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                      .ValueGeneratedOnAdd();   

                entity.Property(p => p.Description)
                      .HasMaxLength(8000)
                      .IsUnicode(false);   

                entity.Property(p => p.Code)
                      .HasMaxLength(8000)
                      .IsUnicode(false);

                entity.Property(p => p.RestrictionType)
                      .HasMaxLength(8000)
                      .IsUnicode(false);

                entity.Property(p => p.Name)
                      .HasMaxLength(256)
                      .IsUnicode(true);   

                entity.Property(p => p.Version)
                      .IsRequired();

                entity.Property(p => p.IsNew)
                      .HasDefaultValue(false);

                entity.Property(p => p.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(p => p.CreatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(p => p.UpdatedBy)
                      .HasMaxLength(128)
                      .IsUnicode(false);

                entity.Property(p => p.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(p => p.UpdatedOn)
                      .IsRequired(false);

            });

        }

    }
}

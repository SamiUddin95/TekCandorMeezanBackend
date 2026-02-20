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
        public DbSet<ChequeDepositListResponseDTO> ChequeDepositListResults { get; set; }


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

            modelBuilder.Entity<ChequeDepositListResponseDTO>().HasNoKey();
        }

    }
}

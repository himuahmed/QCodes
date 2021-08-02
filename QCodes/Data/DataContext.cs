using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QCodes.DbObjects;

namespace QCodes.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<QTag> QTags { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<GlobalMessage> GlobalMessages { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<QTag>().Property(x => x.TagId).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Person>().Property(x => x.PersonId).HasDefaultValueSql("NEWID()");
            //modelBuilder.Entity<BloodRequest>().HasOne<Person>(a => a.Sender).WithMany(m => m.Messages)
            //    .HasForeignKey(u => u.UserId);
        }



    }
}

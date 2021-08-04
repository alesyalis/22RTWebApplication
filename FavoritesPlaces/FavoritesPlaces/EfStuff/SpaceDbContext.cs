using TwenyTwoRT.EfStuff.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff
{
    public class SpaceDbContext : DbContext
    {
        public SpaceDbContext(DbContextOptions options): base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.MyCars)
                .WithOne(x => x.Owner);

            modelBuilder.Entity<Client>()
               .HasMany(x => x.Orders)
               .WithOne(x => x.Client);

            modelBuilder.Entity<User>()
               .HasOne(x => x.Client)
               .WithOne(x => x.User)
               .HasForeignKey<Client>(x => x.ForeignKeyUser);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}

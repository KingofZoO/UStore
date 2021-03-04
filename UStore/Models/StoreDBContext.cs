using System;
using Microsoft.EntityFrameworkCore;

namespace UStore.Models {
    public class StoreDBContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }

        public StoreDBContext(DbContextOptions<StoreDBContext> options) : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            Role adminRole = new Role() { Id = 1, Name = RoleTypes.AdminRole };
            Role userRole = new Role() { Id = 2, Name = RoleTypes.UserRole };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });

            base.OnModelCreating(modelBuilder);
        }
    }
}

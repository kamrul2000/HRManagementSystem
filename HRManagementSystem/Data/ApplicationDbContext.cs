using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HRManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding Role Data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin" },
                new Role { Id = 2, Name = "Admin" },
                new Role { Id = 3, Name = "User" }
            );
            modelBuilder.Entity<Branch>()
        .HasOne(b => b.Subscription)
        .WithMany()
        .HasForeignKey(b => b.SubscriptionId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Subscription)
                .WithMany()
                .HasForeignKey(c => c.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                  .HasOne(b => b.Subscription)
                  .WithMany()
                  .HasForeignKey(b => b.SubscriptionId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                  .HasOne(b => b.Subscription)
                  .WithMany()
                  .HasForeignKey(b => b.SubscriptionId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Designation>()
                  .HasOne(b => b.Subscription)
                  .WithMany()
                  .HasForeignKey(b => b.SubscriptionId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
         .HasOne(e => e.Company)
         .WithMany()
         .HasForeignKey(e => e.CompanyId)
         .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
      .HasOne(e => e.Department)
      .WithMany()
      .HasForeignKey(e => e.DepartmentId)
      .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
        .HasOne(e => e.Designation)
        .WithMany()
        .HasForeignKey(e => e.DesignationId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
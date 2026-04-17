using EInsurance_App.Models;
using Microsoft.EntityFrameworkCore;

namespace EInsurance_App.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InsuranceAgent> InsuranceAgents { get; set; }
        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<EmployeeScheme> EmployeeSchemes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // adding unique contraints
            modelBuilder.Entity<Admin>()
            .HasIndex(a => a.Username).IsUnique();
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email).IsUnique();

            modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Username).IsUnique();
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email).IsUnique();

            modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<InsuranceAgent>()
            .HasIndex(e => e.Username).IsUnique();
            modelBuilder.Entity<InsuranceAgent>()
                .HasIndex(e => e.Email).IsUnique();

            // adding relationships

            // CUSTOMER -> AGENT (Many-to-One)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Agent)
                .WithMany(a => a.Customers)
                .HasForeignKey(c => c.AgentID)
                .OnDelete(DeleteBehavior.Restrict);

            // SCHEME -> PLAN (Many-to-One)
            modelBuilder.Entity<Scheme>()
                .HasOne(s => s.Plan)
                .WithMany(p => p.Schemes)
                .HasForeignKey(s => s.PlanID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Policies)
                .HasForeignKey(p => p.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Scheme)
                .WithMany(s => s.Policies)
                .HasForeignKey(p => p.SchemeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Policy)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.PolicyID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Commission>()
                .HasOne(c => c.Agent)
                .WithMany(a => a.Commissions)
                .HasForeignKey(c => c.AgentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Commission>()
                .HasOne(c => c.Policy)
                .WithMany(p => p.Commissions)
                .HasForeignKey(c => c.PolicyID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<EmployeeScheme>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeSchemes)
                .HasForeignKey(es => es.EmployeeID);

            modelBuilder.Entity<EmployeeScheme>()
                .HasOne(es => es.Scheme)
                .WithMany(s => s.EmployeeSchemes)
                .HasForeignKey(es => es.SchemeID);

            // decimal precision
            modelBuilder.Entity<Policy>()
                .Property(p => p.Premium)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Commission>()
                .Property(c => c.CommissionAmount)
                .HasPrecision(10, 2);
        }
    }
}

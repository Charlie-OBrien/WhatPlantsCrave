using Brawndo_Components.Models;
using Microsoft.EntityFrameworkCore;

namespace Brawndo_Components.Data
{
    public class AdventureWorksContext : DbContext
    {
        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<ProductDescription> ProductDescriptions { get; set; } = null!;
        public DbSet<ProductModel> ProductModels { get; set; } = null!;
        public DbSet<ProductModelProductDescription> ProductModelProductDescriptions { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<CustomerAddress> CustomerAddresses { get; set; } = null!;
        public DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; } = null!;
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; } = null!;
        public DbSet<BuildVersion> BuildVersions { get; set; } = null!;
        public DbSet<ErrorLog> ErrorLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("SalesLT");
        }
    }
}


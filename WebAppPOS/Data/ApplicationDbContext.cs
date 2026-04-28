using Microsoft.EntityFrameworkCore;
using WebAppPOS.Models;

namespace WebAppPOS.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.
        //    UseSqlServer("Server=ROYAL;Database=pos;User Id=sa;Password=Royal@sqlserver;TrustServerCertificate=True;");
        //    base.OnConfiguring(optionsBuilder);
        //}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Purchase -> Supplier
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Suppliers)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Purchase -> Created User
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.CreatedUser)
                .WithMany(u => u.CreatedPurchases)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Purchase -> Updated User
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.UpdatedUser)
                .WithMany(u => u.UpdatedPurchases)
                .HasForeignKey(p => p.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

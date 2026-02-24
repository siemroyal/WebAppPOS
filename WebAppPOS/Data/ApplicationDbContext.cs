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
    }
}

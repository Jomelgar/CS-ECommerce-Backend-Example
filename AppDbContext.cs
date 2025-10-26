using Microsoft.EntityFrameworkCore;
using Models;

namespace ConnectionDb
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set;  }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        /*
        ---Crear modelos con relación de entidades 1-N o demás
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .HasKey(i => i.id);
            modelBuilder.Entity<Product>()
                .HasKey(i => i.id); 
            modelBuilder.Entity<Order>()
                .HasKey(i => i.id);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.id_product)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Order>()
                .HasOne(i => i.Inventory)
                .WithMany(p => p.Orders)
                .HasForeignKey(i => i.id_inventory)
                .OnDelete(DeleteBehavior.Cascade);
        }
        */
    }
}
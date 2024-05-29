using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace backend.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product-Category -- one-to-many
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // User-Address -- one-to-one
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<User>(u => u.AddressId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Restaurant-Address -- one-to-one
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Address)
                .WithOne()
                .HasForeignKey<Restaurant>(r => r.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restaurant-Product -- one-to-many
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Products)
                .WithOne(p => p.Restaurant)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-Restaurant -- one-to-many
            modelBuilder.Entity<User>()
                .HasMany(u => u.Restaurants)
                .WithOne(r => r.Owner)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>(order =>
            {
                order.OwnsMany(o => o.OrderItems, oi =>
                {
                    oi.WithOwner().HasForeignKey("OrderId");
                    oi.Property<int>("Id");
                    oi.HasKey("Id");
                    oi.ToTable("OrderItems");
                });

                order.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Property);
            });

        }
    }
}

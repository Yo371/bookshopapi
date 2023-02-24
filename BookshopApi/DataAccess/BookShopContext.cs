using BookshopApi.Entities;
using Commons.Models;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.DataAccess;

public class BookShopContext : DbContext
{
    public virtual DbSet<ProductEntity> Products { get; set; }
    
    public virtual DbSet<StoreItemEntity> StoreItems { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }
    
    public virtual DbSet<AuthEntity> AuthModels { get; set; }
    
    public virtual DbSet<BookingStatusEntity> BookingStatuses { get; set; }
    
    public virtual DbSet<BookingEntity> Bookings { get; set; }

    public BookShopContext(DbContextOptions<BookShopContext> options) : base(options)
    {
    }

    public BookShopContext()
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder
            .Entity<AuthEntity>()
            .Property(e => e.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v, true));
        
        modelBuilder
            .Entity<BookingStatusEntity>()
            .Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v, true));

        base.OnModelCreating(modelBuilder);

    }
    
}
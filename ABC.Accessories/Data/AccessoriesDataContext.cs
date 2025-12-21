using ABC.Accessories.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace ABC.Accessories.Data;
public class AccessoriesDataContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessoryBase>()
            .HasIndex(x => x.AccessoryBaseId)
            .IsUnique();

        modelBuilder.Entity<Accessory>()
            .HasIndex(x => x.AccessoryGuid)
            .IsUnique();

        modelBuilder.Entity<Accessory>()
            .HasMany(e => e.Sellers)
            .WithMany(e => e.Accessories)
            .UsingEntity("AccessorySellerXREF");
    }

    protected void SetBaseDBProps(NpgsqlDbContextOptionsBuilder builderOptions, string schemaName)
    {
        builderOptions.MigrationsHistoryTable("__EFMigrationsHistory", schemaName);
        builderOptions.CommandTimeout(60);
        builderOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null
        );
    }

    public DbSet<AccessoryBase> AccessoryBase => Set<AccessoryBase>();
    public DbSet<Accessory> Accessories => Set<Accessory>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<ItemImage> ItemImages => Set<ItemImage>();
    public DbSet<Seller> Sellers => Set<Seller>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<DeviceModel> DeviceModel => Set<DeviceModel>();
}

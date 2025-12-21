using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;

public class MobilesDataContext(
                        DbContextOptions<MobilesDataContext> options,
                        IConfiguration config
                    ) : AccessoriesDataContext(options)
{

    private readonly string schemaName = "abc-mobiles";
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(schemaName);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
                    config.GetConnectionString("ABC_Mobiles_DB"),
                    builderOptions => SetBaseDBProps(builderOptions, schemaName)
                );
    }

}
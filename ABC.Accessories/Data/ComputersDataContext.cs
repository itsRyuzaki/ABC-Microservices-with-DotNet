using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;
public class ComputersDataContext(
                        DbContextOptions<ComputersDataContext> options,
                        IConfiguration config
                    ) : AccessoriesDataContext(options)
{

    private readonly string schemaName = "abc-computers";


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(schemaName);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
                    config.GetConnectionString("ABC_Computers_DB"),
                    builderOptions => SetBaseDBProps(builderOptions, schemaName)

                );
    }
}
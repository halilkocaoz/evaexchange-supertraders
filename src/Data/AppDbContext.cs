using EvaExchange.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Share> Shares { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Share>().ToTable("Shares");
        modelBuilder.Entity<Share>().HasKey(x => x.Id);
        modelBuilder.Entity<Share>().Property(x => x.Id).HasMaxLength(3).HasColumnType("char(3)");
        modelBuilder.Entity<Share>().Property(x => x.Rate).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Share>().Property(x => x.Price).HasColumnType("numeric(18,2)");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
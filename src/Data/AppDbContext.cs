using EvaExchange.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvaExchange.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Share> Shares { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserShares> UserShares { get; set; }
    
    public DbSet<Trade> Trades { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Share>().ToTable("Shares");
        modelBuilder.Entity<Share>().HasKey(x => x.Id);
        modelBuilder.Entity<Share>().Property(x => x.CreatorUserId).HasMaxLength(36);
        modelBuilder.Entity<Share>().HasIndex(x => x.CreatorUserId);
        modelBuilder.Entity<Share>().Property(x => x.Id).HasColumnType("char(3)");
        modelBuilder.Entity<Share>().Property(x => x.Rate).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<Share>().Property(x => x.Price).HasColumnType("numeric(18,6)");

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().Property(x => x.Id).HasMaxLength(36);
        modelBuilder.Entity<User>().Property(x => x.FullName).HasMaxLength(255).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(255).IsRequired();
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(1000).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.Balance).HasColumnType("numeric(18,6)");

        modelBuilder.Entity<UserShares>().ToTable("UserShares");
        modelBuilder.Entity<UserShares>().HasKey(x => new {x.UserId, x.ShareId});
        modelBuilder.Entity<UserShares>().Property(x => x.UserId).HasMaxLength(36);
        modelBuilder.Entity<UserShares>().Property(x => x.ShareId).HasColumnType("char(3)");
        modelBuilder.Entity<UserShares>().Property(x => x.Rate).HasColumnType("numeric(18,2)");
        modelBuilder.Entity<UserShares>().HasOne(x => x.User).WithMany(x => x.Shares).HasForeignKey(x => x.UserId);
        modelBuilder.Entity<UserShares>().HasOne(x => x.Share).WithMany().HasForeignKey(x => x.ShareId);

        modelBuilder.Entity<Trade>().ToTable("Trades");
        modelBuilder.Entity<Trade>().HasKey(x => x.Id);
        modelBuilder.Entity<Trade>().Property(x => x.Id).HasMaxLength(36);
        modelBuilder.Entity<Trade>().Property(x => x.UserId).HasMaxLength(36).IsRequired();
        modelBuilder.Entity<Trade>().Property(x => x.ShareId).HasColumnType("char(3)").IsRequired();
        modelBuilder.Entity<Trade>().Property(x => x.Rate).HasColumnType("numeric(18,2)").IsRequired();
        modelBuilder.Entity<Trade>().Property(x => x.Price).HasColumnType("numeric(18,6)").IsRequired();
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
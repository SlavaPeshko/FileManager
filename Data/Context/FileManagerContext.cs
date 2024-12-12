using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class FileManagerContext : DbContext
{
    public FileManagerContext(DbContextOptions<FileManagerContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("User");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name).IsUnique();

            e.Property(u => u.Name).HasColumnType("nvarchar(100)");
            e.Property(u => u.Password).HasColumnType("nvarchar(20)");
        });

        modelBuilder.Entity<Document>(e =>
        {
            e.ToTable("Documents");
            e.HasKey(x => x.Id);

            e.Property(u => u.Name).IsRequired().HasColumnType("nvarchar(100)");
            e.Property(u => u.Path).HasColumnType("nvarchar(100)");
            e.Property(u => u.UploadAt).IsRequired();

            e.HasOne(p => p.User)
                .WithMany(u => u.Documents);
        });

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}
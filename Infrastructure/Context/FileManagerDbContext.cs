using Application.Common.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class FileManagerDbContext: DbContext, IFileManagerDbContext
{
    public FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : base(options)
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

            e.Property(u => u.Name).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
            e.Property(u => u.Password).IsRequired().HasColumnType("nvarchar(20)").HasMaxLength(20);
        });

        modelBuilder.Entity<Document>(e =>
        {
            e.ToTable("Documents");
            e.HasKey(x => x.Id);

            e.Property(u => u.Name).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
            e.Property(u => u.UniqueName).IsRequired().HasColumnType("nvarchar(292)").HasMaxLength(292);
            e.Property(u => u.UploadAt).IsRequired();

            e.HasOne(p => p.User)
                .WithMany(u => u.Documents);
        });

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}
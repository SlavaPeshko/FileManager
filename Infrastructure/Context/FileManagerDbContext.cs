using Data.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class FileManagerDbContext: DbContext, IFileManagerDbContext
{
    public FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }

    public DbSet<SharedLink> SharedLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Name).IsUnique();

            e.Property(u => u.Name).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
            e.Property(u => u.Password).IsRequired().HasColumnType("nvarchar(20)").HasMaxLength(20);
        });

        modelBuilder.Entity<Document>(e =>
        {
            e.ToTable("Documents");
            e.HasKey(d => d.Id);

            e.Property(d => d.Name).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
            e.Property(d => d.UniqueName).IsRequired().HasColumnType("nvarchar(292)").HasMaxLength(292);
            e.Property(d => d.UploadAt).IsRequired();

            e.HasOne(d => d.User)
                .WithMany(d => d.Documents);

            e.HasMany(d => d.SharedLinks)
                .WithOne(sl => sl.Document)
                .HasForeignKey(sl => sl.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<SharedLink>(e =>
        {
            e.ToTable("SharedLinks");
            e.HasKey(s => s.Id);
            
            e.Property(s => s.UniqueKey)
                .IsRequired()
                .HasMaxLength(64);

            e.HasOne(s => s.Document)
                .WithMany(d => d.SharedLinks);
        });

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}
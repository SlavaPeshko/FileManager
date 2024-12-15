using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Interfaces;

public interface IFileManagerDbContext
{
    DbSet<User> Users { get; }

    DbSet<Document> Documents { get; }

    DbSet<SharedLink> SharedLinks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DatabaseFacade Database { get; }
}
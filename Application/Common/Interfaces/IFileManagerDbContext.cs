using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IFileManagerDbContext
{
    DbSet<User> Users { get; }

    DbSet<Document> Documents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
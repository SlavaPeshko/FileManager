using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public static class FileManagerContextSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = 1,
                Name = "admin",
                Password = "admin",
            });
    }
}
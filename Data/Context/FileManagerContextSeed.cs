using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public static class FileManagerContextSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(new User[]
            {
                new User
                {
                    Id = 1,
                    Name = "admin",
                    Password = "admin",
                }
            });
    }
}
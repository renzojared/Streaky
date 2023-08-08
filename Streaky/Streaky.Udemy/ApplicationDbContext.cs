using Microsoft.EntityFrameworkCore;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Author> Author { get; set; }
}


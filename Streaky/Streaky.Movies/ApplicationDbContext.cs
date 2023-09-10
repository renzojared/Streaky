using Microsoft.EntityFrameworkCore;
using Streaky.Movies.Entities;

namespace Streaky.Movies;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions opt) : base(opt)
    {
    }

    public DbSet<Gender> Genders { get; set; }
}


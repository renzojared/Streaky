using Microsoft.EntityFrameworkCore;
using Streaky.Movies.Entities;

namespace Streaky.Movies;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions opt) : base(opt)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MoviesActors>()
            .HasKey(s => new { s.ActorId, s.MovieId });

        modelBuilder.Entity<MoviesGenders>()
            .HasKey(s => new { s.GenderId, s.MovieId });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Gender> Genders { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MoviesActors> MoviesActors { get; set; }
    public DbSet<MoviesGenders> MoviesGenders { get; set; }
}


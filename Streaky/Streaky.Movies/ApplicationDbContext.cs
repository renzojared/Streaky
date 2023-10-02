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

        modelBuilder.Entity<MoviesMovieTheater>()
            .HasKey(s => new { s.MovieId, s.MovieTheaterId });

        SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var adventure = new Gender() { Id = 4, Name = "Aventura" };
        var animation = new Gender() { Id = 5, Name = "Animación" };
        var suspense = new Gender() { Id = 6, Name = "Suspenso" };
        var romance = new Gender() { Id = 7, Name = "Romance" };

        modelBuilder.Entity<Gender>()
            .HasData(new List<Gender>
            {
                    adventure, animation, suspense, romance
            });

        var jimCarrey = new Actor() { Id = 8, Name = "Jim Carrey", BirthDate = new DateTime(1962, 01, 17) };
        var robertDowney = new Actor() { Id = 9, Name = "Robert Downey Jr.", BirthDate = new DateTime(1965, 4, 4) };
        var chrisEvans = new Actor() { Id = 10, Name = "Chris Evans", BirthDate = new DateTime(1981, 06, 13) };

        modelBuilder.Entity<Actor>()
            .HasData(new List<Actor>
            {
                    jimCarrey, robertDowney, chrisEvans
            });

        var endgame = new Movie()
        {
            Id = 4,
            Title = "Avengers: Endgame",
            InCinema = true,
            ReleaseDate = new DateTime(2019, 04, 26)
        };

        var iw = new Movie()
        {
            Id = 5,
            Title = "Avengers: Infinity Wars",
            InCinema = false,
            ReleaseDate = new DateTime(2019, 04, 26)
        };

        var sonic = new Movie()
        {
            Id = 6,
            Title = "Sonic the Hedgehog",
            InCinema = false,
            ReleaseDate = new DateTime(2020, 02, 28)
        };
        var emma = new Movie()
        {
            Id = 7,
            Title = "Emma",
            InCinema = false,
            ReleaseDate = new DateTime(2020, 02, 21)
        };
        var wonderwoman = new Movie()
        {
            Id = 8,
            Title = "Wonder Woman 1984",
            InCinema = false,
            ReleaseDate = new DateTime(2020, 08, 14)
        };

        modelBuilder.Entity<Movie>()
            .HasData(new List<Movie>
            {
                    endgame, iw, sonic, emma, wonderwoman
            });

        modelBuilder.Entity<MoviesGenders>().HasData(
            new List<MoviesGenders>()
            {
                    new MoviesGenders(){MovieId = endgame.Id, GenderId = suspense.Id},
                    new MoviesGenders(){MovieId = endgame.Id, GenderId = adventure.Id},
                    new MoviesGenders(){MovieId = iw.Id, GenderId = suspense.Id},
                    new MoviesGenders(){MovieId = iw.Id, GenderId = adventure.Id},
                    new MoviesGenders(){MovieId = sonic.Id, GenderId = adventure.Id},
                    new MoviesGenders(){MovieId = emma.Id, GenderId = suspense.Id},
                    new MoviesGenders(){MovieId = emma.Id, GenderId = romance.Id},
                    new MoviesGenders(){MovieId = wonderwoman.Id, GenderId = suspense.Id},
                    new MoviesGenders(){MovieId = wonderwoman.Id, GenderId = adventure.Id},
            });

        modelBuilder.Entity<MoviesActors>().HasData(
            new List<MoviesActors>()
            {
                    new MoviesActors(){MovieId = endgame.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = endgame.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = iw.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = iw.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = sonic.Id, ActorId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1}
            });
    }

    public DbSet<Gender> Genders { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MoviesActors> MoviesActors { get; set; }
    public DbSet<MoviesGenders> MoviesGenders { get; set; }
    public DbSet<MovieTheater> movieTheaters { get; set; }
    public DbSet<MoviesMovieTheater> MoviesMovieTheaters { get; set; }
}


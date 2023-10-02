using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using Streaky.Movies.Entities;

namespace Streaky.Movies;

public class ApplicationDbContext : IdentityDbContext
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
        var rolAdminId = "BBA43D23-7E34-4D2F-918A-85D282DB966C";
        var userAdminId = "9596FBB8-805E-4B93-B8D7-EC141FA61061";

        var rolAdmin = new IdentityRole()
        {
            Id = rolAdminId,
            Name = "Admin",
            NormalizedName = "Admin"
        };
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var username = "renzojared_lm@hotmail.com";

        var userAdmin = new IdentityUser()
        {
            Id = userAdminId,
            UserName = username,
            NormalizedUserName = username,
            Email = username,
            NormalizedEmail = username,
            PasswordHash = passwordHasher.HashPassword(null, "rleon@@")
        };

        modelBuilder.Entity<IdentityUser>()
            .HasData(userAdmin);

        modelBuilder.Entity<IdentityRole>()
            .HasData(rolAdmin);

        modelBuilder.Entity<IdentityUserClaim<string>>()
            .HasData(new IdentityUserClaim<string>()
            {
                Id = 1,
                ClaimType = ClaimTypes.Role,
                UserId = userAdminId,
                ClaimValue = "Admin"
            });

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        modelBuilder.Entity<MovieTheater>()
            .HasData(new List<MovieTheater>
            {
                new MovieTheater{Id = 2, Name = "Mall de Comas", Location = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-11.994766,-11.994766))},
                new MovieTheater{Id = 3, Name = "Mega Plaza", Location = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-77.062870,-11.994766))},
                new MovieTheater{Id = 4, Name = "Espania Mall", Location = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-105.619548,32.675991

))}
            });

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
    public DbSet<MovieTheater> MovieTheaters { get; set; }
    public DbSet<MoviesMovieTheater> MoviesMovieTheaters { get; set; }
}


﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Streaky.Movies;

#nullable disable

namespace Streaky.Movies.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231002024405_withdataMovieTheaters")]
    partial class withdataMovieTheaters
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Streaky.Movies.Entities.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Actors");

                    b.HasData(
                        new
                        {
                            Id = 8,
                            BirthDate = new DateTime(1962, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Jim Carrey"
                        },
                        new
                        {
                            Id = 9,
                            BirthDate = new DateTime(1965, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Robert Downey Jr."
                        },
                        new
                        {
                            Id = 10,
                            BirthDate = new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chris Evans"
                        });
                });

            modelBuilder.Entity("Streaky.Movies.Entities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Genders");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            Name = "Aventura"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Animación"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Suspenso"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Romance"
                        });
                });

            modelBuilder.Entity("Streaky.Movies.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("InCinema")
                        .HasColumnType("bit");

                    b.Property<string>("Poster")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            InCinema = true,
                            ReleaseDate = new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Avengers: Endgame"
                        },
                        new
                        {
                            Id = 5,
                            InCinema = false,
                            ReleaseDate = new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Avengers: Infinity Wars"
                        },
                        new
                        {
                            Id = 6,
                            InCinema = false,
                            ReleaseDate = new DateTime(2020, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Sonic the Hedgehog"
                        },
                        new
                        {
                            Id = 7,
                            InCinema = false,
                            ReleaseDate = new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Emma"
                        },
                        new
                        {
                            Id = 8,
                            InCinema = false,
                            ReleaseDate = new DateTime(2020, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Wonder Woman 1984"
                        });
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MovieTheater", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Point>("Location")
                        .IsRequired()
                        .HasColumnType("geography");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.HasKey("Id");

                    b.ToTable("MovieTheaters");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Location = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-11.994766 -11.994766)"),
                            Name = "Mall de Comas"
                        },
                        new
                        {
                            Id = 3,
                            Location = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-77.06287 -11.994766)"),
                            Name = "Mega Plaza"
                        },
                        new
                        {
                            Id = 4,
                            Location = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-105.619548 32.675991)"),
                            Name = "Espania Mall"
                        });
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MoviesActors", b =>
                {
                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<string>("Character")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("ActorId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("MoviesActors");

                    b.HasData(
                        new
                        {
                            ActorId = 9,
                            MovieId = 4,
                            Character = "Tony Stark",
                            Order = 1
                        },
                        new
                        {
                            ActorId = 10,
                            MovieId = 4,
                            Character = "Steve Rogers",
                            Order = 2
                        },
                        new
                        {
                            ActorId = 9,
                            MovieId = 5,
                            Character = "Tony Stark",
                            Order = 1
                        },
                        new
                        {
                            ActorId = 10,
                            MovieId = 5,
                            Character = "Steve Rogers",
                            Order = 2
                        },
                        new
                        {
                            ActorId = 8,
                            MovieId = 6,
                            Character = "Dr. Ivo Robotnik",
                            Order = 1
                        });
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MoviesGenders", b =>
                {
                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.HasKey("GenderId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("MoviesGenders");

                    b.HasData(
                        new
                        {
                            GenderId = 6,
                            MovieId = 4
                        },
                        new
                        {
                            GenderId = 4,
                            MovieId = 4
                        },
                        new
                        {
                            GenderId = 6,
                            MovieId = 5
                        },
                        new
                        {
                            GenderId = 4,
                            MovieId = 5
                        },
                        new
                        {
                            GenderId = 4,
                            MovieId = 6
                        },
                        new
                        {
                            GenderId = 6,
                            MovieId = 7
                        },
                        new
                        {
                            GenderId = 7,
                            MovieId = 7
                        },
                        new
                        {
                            GenderId = 6,
                            MovieId = 8
                        },
                        new
                        {
                            GenderId = 4,
                            MovieId = 8
                        });
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MoviesMovieTheater", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("MovieTheaterId")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "MovieTheaterId");

                    b.HasIndex("MovieTheaterId");

                    b.ToTable("MoviesMovieTheaters");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MoviesActors", b =>
                {
                    b.HasOne("Streaky.Movies.Entities.Actor", "Actor")
                        .WithMany("MoviesActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streaky.Movies.Entities.Movie", "Movie")
                        .WithMany("MoviesActors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MoviesGenders", b =>
                {
                    b.HasOne("Streaky.Movies.Entities.Gender", "Gender")
                        .WithMany("MoviesGenders")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streaky.Movies.Entities.Movie", "Movie")
                        .WithMany("MoviesGenders")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gender");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MoviesMovieTheater", b =>
                {
                    b.HasOne("Streaky.Movies.Entities.Movie", "Movie")
                        .WithMany("MoviesMovieTheaters")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streaky.Movies.Entities.MovieTheater", "MovieTheater")
                        .WithMany("MoviesMovieTheaters")
                        .HasForeignKey("MovieTheaterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("MovieTheater");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.Actor", b =>
                {
                    b.Navigation("MoviesActors");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.Gender", b =>
                {
                    b.Navigation("MoviesGenders");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.Movie", b =>
                {
                    b.Navigation("MoviesActors");

                    b.Navigation("MoviesGenders");

                    b.Navigation("MoviesMovieTheaters");
                });

            modelBuilder.Entity("Streaky.Movies.Entities.MovieTheater", b =>
                {
                    b.Navigation("MoviesMovieTheaters");
                });
#pragma warning restore 612, 618
        }
    }
}

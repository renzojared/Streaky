using AutoMapper;
using Streaky.Movies.DTOs;
using Streaky.Movies.Entities;

namespace Streaky.Movies.Helper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Gender, GenderDTO>().ReverseMap();
        CreateMap<GenderCreationDTO, Gender>().ReverseMap();

        CreateMap<MovieTheater, MovieTheaterDTO>().ReverseMap();
        CreateMap<MovieTheaterCreationDTO, MovieTheater>().ReverseMap();

        CreateMap<Actor, ActorDTO>().ReverseMap();
        CreateMap<ActorCreationDTO, Actor>()
            .ForMember(s => s.Photo, opt => opt.Ignore());

        CreateMap<ActorPatchDTO, Actor>().ReverseMap();

        CreateMap<Movie, MovieDTO>().ReverseMap();

        CreateMap<MovieCreationDTO, Movie>()
            .ForMember(s => s.Poster, opt => opt.Ignore())
            .ForMember(s => s.MoviesGenders, opt => opt.MapFrom(MapMoviesGenders))
            .ForMember(s => s.MoviesActors, opt => opt.MapFrom(MapMoviesActors));

        CreateMap<Movie, MovieDetailDTO>()
            .ForMember(s => s.Genders, opt => opt.MapFrom(MapMoviesGender))
            .ForMember(s => s.Actors, opt => opt.MapFrom(MapMoviesActors));

        CreateMap<MoviePatchDTO, Movie>().ReverseMap();
    }

    private List<ActorMovieDetailDTO> MapMoviesActors(Movie movie, MovieDetailDTO movieDetailDTO)
    {
        var result = new List<ActorMovieDetailDTO>();

        if (movie.MoviesActors is null)
            return result;

        foreach (var actorMovie in movie.MoviesActors)
        {
            result.Add(new ActorMovieDetailDTO
            {
                ActorId = actorMovie.ActorId,
                Character = actorMovie.Character,
                PersonName = actorMovie.Actor.Name
            });
        }
        return result;
    }

    private List<GenderDTO> MapMoviesGender(Movie movie, MovieDetailDTO movieDetailDTO)
    {
        var result = new List<GenderDTO>();
        if (movieDetailDTO is null)
            return result;

        foreach (var genderMovie in movie.MoviesGenders)
        {
            result.Add(new GenderDTO() { Id = genderMovie.GenderId, Name = genderMovie.Gender.Name });
        }

        return result;
    }

    private List<MoviesGenders> MapMoviesGenders(MovieCreationDTO movieCreationDTO, Movie movie)
    {
        var result = new List<MoviesGenders>();

        if (movieCreationDTO.GendersIds is null)
            return result;

        foreach (var id in movieCreationDTO.GendersIds)
        {
            result.Add(new MoviesGenders() { GenderId = id });
        }

        return result;
    }

    private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
    {
        var result = new List<MoviesActors>();

        if (movieCreationDTO.GendersIds is null)
            return result;

        foreach (var actor in movieCreationDTO.Actors)
        {
            result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character });
        }

        return result;
    }
}


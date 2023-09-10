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

        CreateMap<Actor, ActorDTO>().ReverseMap();
        CreateMap<ActorCreationDTO, Actor>()
            .ForMember(s => s.Photo, opt => opt.Ignore());

        CreateMap<ActorPatchDTO, Actor>().ReverseMap();

        CreateMap<Movie, MovieDTO>().ReverseMap();
        CreateMap<MovieCreationDTO, Movie>()
            .ForMember(s => s.Poster, opt => opt.Ignore())
            .ForMember(s => s.MoviesGenders, opt => opt.MapFrom(MapMoviesGenders))
            .ForMember(s => s.MoviesActors, opt => opt.MapFrom(MapMoviesActors));

        CreateMap<MoviePatchDTO, Movie>().ReverseMap();


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


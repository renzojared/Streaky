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
        CreateMap<ActorCreationDTO, Actor>().ReverseMap();
    }
}


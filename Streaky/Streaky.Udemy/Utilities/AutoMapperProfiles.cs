using AutoMapper;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AuthorCreationDTO, Author>().ReverseMap();
        CreateMap<Author, AuthorDTO>().ReverseMap();
        CreateMap<BookCreationDTO, Book>().ReverseMap();
        CreateMap<Book, BookDTO>().ReverseMap();
    }
}


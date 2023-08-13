using AutoMapper;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Entities;

namespace Streaky.Udemy.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AuthorCreationDTO, Author>().ReverseMap();
        CreateMap<Author, AuthorDTO>();

        CreateMap<Author, AuthorDTOWithBook>()
            .ForMember(_ => _.Books, opt => opt.MapFrom(MapAuthorDTOBooks));

        CreateMap<BookCreationDTO, Book>()
            .ForMember(book => book.AuthorBooks, opt => opt.MapFrom(MapAuthorsBooks));

        CreateMap<Book, BookDTO>();

        CreateMap<Book, BookDTOWithAuthor>()
            .ForMember(bDTO => bDTO.Authors, opt => opt.MapFrom(MapBookDTOAuthors));

        CreateMap<BookPatchDTO, Book>().ReverseMap();

        CreateMap<CommentCreationDTO, Comment>().ReverseMap();
        CreateMap<CommentDTO, Comment>().ReverseMap();

    }

    private List<BookDTO> MapAuthorDTOBooks(Author author, AuthorDTO authorDTO)
    {
        var result = new List<BookDTO>();

        if(author.AuthorBooks == null) { return result; }

        foreach(var authorBook in author.AuthorBooks)
        {
            result.Add(new BookDTO()
            {
                Id = authorBook.BookId,
                Tittle = authorBook.Book.Tittle
            });
        }

        return result;
    }

    private List<AuthorDTO> MapBookDTOAuthors(Book book, BookDTO bookDTO)
    {
        var result = new List<AuthorDTO>();

        if(book.AuthorBooks == null) { return result; }

        foreach(var authorBook in book.AuthorBooks)
        {
            result.Add(new AuthorDTO()
            {
                Id = authorBook.AuthorId,
                Name = authorBook.Author.Name
            });
        }
        return result;
    }

    private List<AuthorBook> MapAuthorsBooks(BookCreationDTO bookCreationDTO, Book book)
    {
        var result = new List<AuthorBook>();

        if(bookCreationDTO.AuthorsId == null) { return result; }

        foreach(var authorId in bookCreationDTO.AuthorsId)
        {
            result.Add(new AuthorBook() { AuthorId = authorId });
        }

        return result;
    }
}


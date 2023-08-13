namespace Streaky.Udemy.DTOs;

public class BookDTOWithAuthor : BookDTO
{
    public List<AuthorDTO> Authors { get; set; }
}


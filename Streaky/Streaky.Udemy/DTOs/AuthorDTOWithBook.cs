namespace Streaky.Udemy.DTOs;

public class AuthorDTOWithBook : AuthorDTO
{
    public List<BookDTO> Books { get; set; }
}


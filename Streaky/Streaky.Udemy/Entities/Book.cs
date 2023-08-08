namespace Streaky.Udemy.Entities;

public class Book
{
    public int Id { get; set; }
    public string Tittle { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
}


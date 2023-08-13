using System.ComponentModel.DataAnnotations;
using Streaky.Udemy.Validator;

namespace Streaky.Udemy.Entities;

public class Book
{
    public int Id { get; set; }
    [Required]
    [FirstCapitalLetter]
    [StringLength(maximumLength:100)]
    public string Tittle { get; set; }
    public DateTime? PublicationDate { get; set; }
    public List<Comment> Comments { get; set; }
    public List<AuthorBook> AuthorBooks { get; set; }
}


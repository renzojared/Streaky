using System.ComponentModel.DataAnnotations;
using Streaky.Udemy.Validator;

namespace Streaky.Udemy.Entities;

public class Book
{
    public int Id { get; set; }
    [FirstCapitalLetter]
    [StringLength(maximumLength:100)]
    public string Tittle { get; set; }
}


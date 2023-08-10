using Streaky.Udemy.Validator;
using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.DTOs;

public class BookCreationDTO
{
    [FirstCapitalLetter]
    [StringLength(maximumLength: 100)]
    public string Tittle { get; set; }
}


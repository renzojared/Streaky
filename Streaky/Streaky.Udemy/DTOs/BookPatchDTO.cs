using Streaky.Udemy.Validator;
using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.DTOs;

public class BookPatchDTO
{
    [FirstCapitalLetter]
    [StringLength(maximumLength: 100)]
    [Required]
    public string Tittle { get; set; }
    public DateTime PublicationDate { get; set; }
}


using Streaky.Udemy.Validator;
using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.DTOs;

public class AuthorCreationDTO
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe tener más de {1} caractéres.")]
    [FirstCapitalLetter]
    public string Name { get; set; }
}


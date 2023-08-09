using System.ComponentModel.DataAnnotations;
using Streaky.Udemy.Validator;

namespace Streaky.Udemy.Entities;

public class Author
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength:50, ErrorMessage = "El campo {0} no debe tener más de {1} caractéres.")]
    [FirstCapitalLetter]
    public string Name { get; set; }
}


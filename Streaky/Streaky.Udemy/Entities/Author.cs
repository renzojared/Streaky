using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streaky.Udemy.Validator;

namespace Streaky.Udemy.Entities;

public class Author : IValidatableObject //Validacion a nivel de modelo
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength:50, ErrorMessage = "El campo {0} no debe tener más de {1} caractéres.")]
    //[FirstCapitalLetter]
    public string Name { get; set; }
    /*[Range(18, 120)]
    [NotMapped]
    public int Age { get; set; }
    [CreditCard]
    [NotMapped]
    public string CreditCard { get; set; }
    [Url]
    [NotMapped]
    public string Url { get; set; }
    [NotMapped]
    public int less { get; set; }
    [NotMapped]
    public int major { get; set; }*/
    public List<Book> Books { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            var firstLetter = Name[0].ToString();

            if (firstLetter != firstLetter.ToUpper())
            {
                yield return new ValidationResult("La primera letra debe ser mayuscula",
                    new string[] { nameof(Name) });
            }
        }
        /*if (less > major)
            yield return new ValidationResult("Este valor menor no puede ser mas grande que el campo mayor",
                new string[] { nameof(less) });*/
    }
}


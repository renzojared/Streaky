using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.Validator;

public class FirstCapitalLetterAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
            return ValidationResult.Success;

        var firtsLetter = value.ToString()[0].ToString();

        if (firtsLetter != firtsLetter.ToUpper())
            return new ValidationResult("La primera letra debe ser mayuscula.");

        return ValidationResult.Success;
    }
}


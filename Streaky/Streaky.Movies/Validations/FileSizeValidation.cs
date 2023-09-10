using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.Validations;

public class FileSizeValidation : ValidationAttribute
{
    private readonly int maxSizeInMegaBytes;

    public FileSizeValidation(int maxSizeInMegaBytes)
    {
        this.maxSizeInMegaBytes = maxSizeInMegaBytes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        IFormFile formFile = value as IFormFile;

        if (formFile == null)
            return ValidationResult.Success;

        if (formFile.Length > maxSizeInMegaBytes * 1024 * 1024)
            return new ValidationResult($"El peso del archivo no debe ser mayor a {maxSizeInMegaBytes} mb");

        return ValidationResult.Success;
    }
}


using System.ComponentModel.DataAnnotations;
using Streaky.Movies.Validations;

namespace Streaky.Movies.DTOs;

public class ActorCreationDTO
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    [FileSizeValidation(maxSizeInMegaBytes: 4)]
    [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
    public IFormFile Photo { get; set; }
}


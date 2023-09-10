using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.DTOs;

public class GenderCreationDTO
{
    [Required]
    [StringLength(40)]
    public string Name { get; set; }
}


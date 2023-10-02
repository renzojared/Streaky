using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.DTOs;

public class MovieTheaterCreationDTO
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
}


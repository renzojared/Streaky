using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.DTOs;

public class ActorPatchDTO
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}


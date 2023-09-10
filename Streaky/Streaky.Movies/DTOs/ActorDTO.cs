using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.DTOs;

public class ActorDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Photo { get; set; }
}


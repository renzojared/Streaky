using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.DTOs;

public class MoviePatchDTO
{
    [Required]
    [StringLength(300)]
    public string Title { get; set; }
    public bool InCinema { get; set; }
    public DateTime ReleaseDate { get; set; }
}


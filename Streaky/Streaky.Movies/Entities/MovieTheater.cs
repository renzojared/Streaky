using System.ComponentModel.DataAnnotations;

namespace Streaky.Movies.Entities;

public class MovieTheater : IId
{
    public int Id { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    public List<MoviesMovieTheater> MoviesMovieTheaters { get; set; }
}


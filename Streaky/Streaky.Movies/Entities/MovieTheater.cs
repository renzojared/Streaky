using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Streaky.Movies.Entities;

public class MovieTheater : IId
{
    public int Id { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    [Column(TypeName = "geography")]
    public Point Location { get; set; }
    public List<MoviesMovieTheater> MoviesMovieTheaters { get; set; }
}


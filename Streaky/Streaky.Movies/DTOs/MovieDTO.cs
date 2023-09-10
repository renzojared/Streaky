namespace Streaky.Movies.DTOs;

public class MovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool InCinema { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Poster { get; set; }
}


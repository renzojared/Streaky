namespace Streaky.Movies.Entities;

public class MoviesActors
{
    public int ActorId { get; set; }
    public int MovieId { get; set; }
    public string Character { get; set; }
    public int Order { get; set; }
    public Actor Actor { get; set; }
    public Movie Movie { get; set; }
}


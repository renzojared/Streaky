namespace Streaky.Movies.DTOs;

public class FilterMoviesDTO
{
    public int Page { get; set; } = 1;
    public int QuantityRecordsByPage { get; set; } = 10;
    public PaginationDTO Pagination
    {
        get { return new PaginationDTO() { Page = Page, QuantityRecordByPage = QuantityRecordsByPage }; }
    }

    public string? Title { get; set; }
    public int GenderId { get; set; }
    public bool InCinema { get; set; }
    public bool NextReleases { get; set; }
    public string FieldOrder { get; set; }
    public bool OrderAscending { get; set; } = true;
}


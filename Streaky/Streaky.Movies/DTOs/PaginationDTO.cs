namespace Streaky.Movies.DTOs;

public class PaginationDTO
{
    public int Page { get; set; }
    private int quantityRecordsByPage = 10;
    private readonly int quantityMaxRecordsByPage = 50;

    public int QuantityRecordByPage { get => quantityRecordsByPage; set
        {
            quantityRecordsByPage = (value > quantityMaxRecordsByPage) ? quantityMaxRecordsByPage : value;
        }}
}


namespace Streaky.Udemy.DTOs;

public class PaginationDTO
{
    public int Page { get; set; }
    private int RecordByPage = 10;
    private readonly int MaxQuantityByPage = 50;

    public int RecordsByPage
    {
        get { return RecordByPage; }
        set { RecordByPage = (value > MaxQuantityByPage) ? MaxQuantityByPage : value; }
    }
}


namespace Streaky.Udemy.DTOs;

public class DataHATEOAS
{
    public string Link { get; private set; }
    public string Description { get; private set; }
    public string Method { get; private set; }

    public DataHATEOAS(string link, string description, string method)
    {
        Link = link;
        Description = description;
        Method = method;
    }
}


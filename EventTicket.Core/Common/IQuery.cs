namespace EventTicket.Core.Common.Paging;

public interface IQuery
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string? Search { get; set; }
    string? SortBy { get; set; }
    bool SortDescending { get; set; }
}

public class BaseQuery : IQuery
{
    private int _pageNumber = 1;
    private int _pageSize = 12;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : (value < 1 ? 1 : value);
    }

    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}

public class EventQuery : BaseQuery
{
    public string? City { get; set; }
    public string? Genre { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public decimal? MaxPrice { get; set; }
}
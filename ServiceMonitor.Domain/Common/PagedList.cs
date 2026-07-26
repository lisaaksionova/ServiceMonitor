namespace ServiceMonitor.Domain.Common;

public class PagedList<T>(List<T> items, int count, int pageNumber, int pageSize)
{
    public List<T> Items { get; set; } = items;
    public int Count { get; set; } = count;
    public int CurrentPage { get; set; } = pageNumber;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
}

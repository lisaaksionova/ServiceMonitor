namespace ServiceMonitor.Domain.Common;

public class CursorPagedList<T>(List<T> items, string? nextCursor, bool hasMore)
{
    public List<T> Items { get; set; } = items;
    public string NextCursor { get; set; } = nextCursor ?? string.Empty;
    public bool HasMore { get; set; } = hasMore;
}

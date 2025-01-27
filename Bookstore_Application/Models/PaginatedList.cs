namespace Bookstore_Application.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public PaginatedList(IEnumerable<T> items,int count, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        
    }
}
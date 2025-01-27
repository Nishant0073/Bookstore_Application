namespace Bookstore_Application.Models;

public class PaginationList<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public PaginationList(IEnumerable<T> items,int count, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        
    }
}
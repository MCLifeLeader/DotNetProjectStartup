namespace Startup.Common.Models;

public class PagedObjectData<TEntityType>
{
    /// <summary>
    /// The collection of records to be returned
    /// </summary>
    public IList<TEntityType> EntityList { get; set; } = new List<TEntityType>();

    /// <summary>
    /// The number of records found in the table
    /// </summary>
    public long TotalItemCount { get; set; }

    /// <summary>
    /// Total Number of records processed thus far
    /// </summary>
    public long TotalPageCount
    {
        get
        {
            return (long)Math.Ceiling(TotalItemCount / (double)PageSize);
        }
    }

    /// <summary>
    /// The specified Page Size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The current page number
    /// </summary>
    public int CurrentPage { get; set; }

}
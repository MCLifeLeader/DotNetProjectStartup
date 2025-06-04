namespace Startup.Data.Models;

public class PagedObjectData<TEntityType>
{
    public IList<TEntityType> EntityList { get; set; } = new List<TEntityType>();
    public long TotalRecordsInTable { get; set; }
    public long PageSize { get; set; }

    // ReSharper disable once UnusedMember.Global
    public long PageCount
    {
        get
        {
            if (EntityList != null && EntityList.Any())
            {
                return EntityList.Count;
            }

            return 0;
        }
    }

    // ReSharper disable once UnusedMember.Global
    public TEntityType? NextStartingId
    {
        get
        {
            if (EntityList != null && EntityList.Any())
            {
                return EntityList.Last();
            }

            return default;
        }
    }
}
using Startup.Data.Models;
using System.Linq.Expressions;

namespace Startup.Data.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType"></typeparam>
public interface ILookupRepositoryTemplate<TEntityType, in TKeyType>
{
    /// <summary>
    ///     Reads the specified record by the <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The <typeparamref name="TEntityType" /> Record.</returns>
    TEntityType GetEntityById(TKeyType key);

    /// <summary>
    ///     Reads the specified record by the <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The <typeparamref name="TEntityType" /> Record.</returns>
    Task<TEntityType> GetEntityByIdAsync(TKeyType key);

    /// <summary>
    ///     Gets all records.
    /// </summary>
    /// <returns>A collection of all <typeparamref name="TEntityType" /> records.</returns>
    IList<TEntityType> GetAll();


    /// <summary>
    ///     Gets all records async
    /// </summary>
    /// <returns>A collection of all <typeparamref name="TEntityType" /> records.</returns>
    Task<IList<TEntityType>> GetAllAsync();

    /// <summary>
    ///     Uses EF based Cursor paging.
    ///     To return the first record in the table, assuming the first Id = 1, set key = 0
    ///     List is in sorted order by Id
    ///     return where (Id > key)
    /// </summary>
    /// <param name="key">The starting record Id less than the current record in the database.</param>
    /// <param name="pageSize">Number of records to return at one time</param>
    /// <returns>Tuple(MembersList, TotalRecords)</returns>
    PagedObjectData<TEntityType> GetByPaging(TKeyType key, int pageSize);

    /// <summary>
    ///     Uses EF based Cursor paging.
    ///     To return the first record in the table, assuming the first Id = 1, set key = 0
    ///     List is in sorted order by Id
    ///     return where (Id > key)
    /// </summary>
    /// <param name="key">The starting record Id less than the current record in the database.</param>
    /// <param name="pageSize">Number of records to return at one time</param>
    /// <returns>Tuple(MembersList, TotalRecords)</returns>
    Task<PagedObjectData<TEntityType>> GetByPagingAsync(TKeyType key, int pageSize);

    /// <summary>
    ///     Gets a queryable collection of records based on the
    ///     <param name="filter" />
    ///     provided.
    /// </summary>
    /// <returns>A filtered collection of <typeparamref name="TEntityType" /> records.</returns>
    IQueryable<TEntityType> Query(Expression<Func<TEntityType, bool>> filter);

    /// <summary>
    ///     Returns a query object.
    /// </summary>
    /// <returns>IQueryable<TEntityType /></returns>
    IQueryable<TEntityType> GetAsQueryable();
}
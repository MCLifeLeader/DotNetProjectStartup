using System.Linq.Expressions;

namespace Startup.Data.Repositories.Interfaces;

/// <summary>
/// This template is to be used if there are conjoined primary keys on the table
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
public interface ILookupRepositoryTemplateDualKey<TEntityType, in TKeyType1, in TKeyType2>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    TEntityType GetEntityById(TKeyType1 key1, TKeyType2 key2);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key1"></param>
    /// <returns></returns>
    IList<TEntityType> GetEntityByFirstKeyId(TKeyType1 key1);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key2"></param>
    /// <returns></returns>
    IList<TEntityType> GetEntityBySecondKeyId(TKeyType2 key2);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    Task<TEntityType> GetEntityByIdAsync(TKeyType1 key1, TKeyType2 key2);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key1"></param>
    /// <returns></returns>
    Task<IList<TEntityType>> GetEntityByFirstKeyIdAsync(TKeyType1 key1);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key2"></param>
    /// <returns></returns>
    Task<IList<TEntityType>> GetEntityBySecondKeyIdAsync(TKeyType2 key2);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IList<TEntityType> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IList<TEntityType>> GetAllAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    IQueryable<TEntityType> Query(Expression<Func<TEntityType, bool>> filter);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntityType> GetAsQueryable();
}
namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
public interface IRepositoryTemplate<TEntityType>
{
    /// <summary>
    ///     Creates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Add(TEntityType entity);

    /// <summary>
    ///     Creates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    Task AddAsync(TEntityType entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    void AddRange(IList<TEntityType> entities);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task AddRangeAsync(IList<TEntityType> entities);

    /// <summary>
    ///     Deletes the specified key.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Delete(TEntityType entity);

    /// <summary>
    ///     Addors the update.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Update(TEntityType entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    void UpdateRange(IList<TEntityType> entities);
}
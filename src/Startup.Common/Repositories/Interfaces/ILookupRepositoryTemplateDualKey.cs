namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// This template is to be used if there are conjoined primary keys on the table
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
public interface ILookupRepositoryTemplateDualKey<TEntityType, in TKeyType1, in TKeyType2> :
    ILookupRepositoryTemplate<TEntityType, TKeyType1>
{
    /// <summary>
    /// To be used when pulling back dual key single records
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    TEntityType GetEntityById(TKeyType1 key1, TKeyType2 key2);

    /// <summary>
    /// To be used when pulling back dual key single records
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
    IList<TEntityType> GetEntitiesByFirstKeyId(TKeyType1 key1);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key1"></param>
    /// <returns></returns>
    Task<IList<TEntityType>> GetEntitiesByFirstKeyIdAsync(TKeyType1 key1);

    /// <summary>
    /// This will return records based on the secondary primary key Id
    /// </summary>
    /// <param name="key2"></param>
    /// <returns></returns>
    IList<TEntityType> GetEntitiesBySecondKeyId(TKeyType2 key2);

    /// <summary>
    /// This will return records based on the secondary primary key Id
    /// </summary>
    /// <param name="key2"></param>
    /// <returns></returns>
    Task<IList<TEntityType>> GetEntitiesBySecondKeyIdAsync(TKeyType2 key2);
}
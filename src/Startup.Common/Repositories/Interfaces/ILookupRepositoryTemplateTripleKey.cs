namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// This template is to be used if there are conjoined primary keys on the table
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
/// <typeparam name="TKeyType3"></typeparam>
public interface ILookupRepositoryTemplateTripleKey<TEntityType, in TKeyType1, in TKeyType2, in TKeyType3> :
    ILookupRepositoryTemplateDualKey<TEntityType, TKeyType1, TKeyType2>
{
    /// <summary>
    /// To be used when pulling back triple key single records
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <param name="key3"></param>
    /// <returns></returns>
    TEntityType GetEntityById(TKeyType1 key1, TKeyType2 key2, TKeyType3 key3);

    /// <summary>
    /// To be used when pulling back triple key single records
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <param name="key3"></param>
    /// <returns></returns>
    Task<TEntityType> GetEntityByIdAsync(TKeyType1 key1, TKeyType2 key2, TKeyType3 key3);

    /// <summary>
    /// This will return records based on the third primary key Id
    /// </summary>
    /// <param name="key3"></param>
    /// <returns></returns>
    IList<TEntityType> GetEntitiesByThirdKeyId(TKeyType3 key3);

    /// <summary>
    /// This will return records based on the third primary key Id
    /// </summary>
    /// <param name="key3"></param>
    /// <returns></returns>
    Task<IList<TEntityType>> GetEntitiesByThirdKeyIdAsync(TKeyType3 key3);
}
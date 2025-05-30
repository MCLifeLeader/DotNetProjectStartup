namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType">The type of the entity type.</typeparam>
/// <typeparam name="TKeyType">The type of the key type.</typeparam>
public interface ILookupNonStructRepository<TEntityType, in TKeyType> 
    : ILookupRepositoryTemplate<TEntityType, TKeyType>
{
}
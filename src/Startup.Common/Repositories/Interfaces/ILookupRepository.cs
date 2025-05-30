namespace Startup.Common.Repositories.Interfaces;

/// <summary>
///     Common Lookup tables Repository Interface
///     Read
/// </summary>
/// <typeparam name="TEntityType">The type of the entity type.</typeparam>
/// <typeparam name="TKeyType">The type of the key type.</typeparam>
public interface ILookupRepository<TEntityType, in TKeyType> 
    : ILookupRepositoryTemplate<TEntityType, TKeyType> where TKeyType : struct
{
}
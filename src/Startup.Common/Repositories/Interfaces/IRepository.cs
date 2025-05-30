namespace Startup.Common.Repositories.Interfaces;

/// <summary>
///     Common Repository Interface
///     Add, Delete, Update
/// </summary>
/// <typeparam name="TEntityType">The type of the entity type.</typeparam>
/// <typeparam name="TKeyType">The type of the key type.</typeparam>
public interface IRepository<TEntityType, in TKeyType> : IRepositoryTemplate<TEntityType>,
    ILookupRepository<TEntityType, TKeyType> where TKeyType : struct
{
}
namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
public interface INonStructRepositoryDualKey<TEntityType, in TKeyType1, in TKeyType2>
    : IRepositoryTemplate<TEntityType>, ILookupNonStructRepositoryDualKey<TEntityType, TKeyType1, TKeyType2>
{
}
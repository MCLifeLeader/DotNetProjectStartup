namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// This is to be used if there are conjoined primary keys on the table
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
public interface IRepositoryDualKey<TEntityType, in TKeyType1, in TKeyType2> 
    : IRepositoryTemplate<TEntityType>, ILookupRepositoryDualKey<TEntityType, TKeyType1, TKeyType2> 
    where TKeyType1 : struct where TKeyType2 : struct
{
}
namespace Startup.Common.Repositories.Interfaces;

public interface IRepositoryTripleKey<TEntityType, in TKeyType1, in TKeyType2, in TKeyType3>
    : IRepositoryTemplate<TEntityType>, ILookupRepositoryTripleKey<TEntityType, TKeyType1, TKeyType2, TKeyType3>
    where TKeyType1 : struct where TKeyType2 : struct where TKeyType3 : struct
{
}
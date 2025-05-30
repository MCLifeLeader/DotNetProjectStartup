namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
/// <typeparam name="TKeyType3"></typeparam>
public interface ILookupRepositoryTripleKey<TEntityType, in TKeyType1, in TKeyType2, in TKeyType3>
    : ILookupRepositoryTemplateTripleKey<TEntityType, TKeyType1, TKeyType2, TKeyType3> 
    where TKeyType1 : struct where TKeyType2 : struct where TKeyType3 : struct
{
}
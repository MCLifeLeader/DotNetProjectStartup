namespace Startup.Common.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
public interface ILookupNonStructRepositoryDualKey<TEntityType, in TKeyType1, in TKeyType2> 
    : ILookupRepositoryTemplateDualKey<TEntityType, TKeyType1, TKeyType2>
{
}
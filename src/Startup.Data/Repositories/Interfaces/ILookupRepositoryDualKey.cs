﻿namespace Startup.Data.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
/// <typeparam name="TKeyType1"></typeparam>
/// <typeparam name="TKeyType2"></typeparam>
public interface ILookupRepositoryDualKey<TEntityType, in TKeyType1, in TKeyType2>
    : ILookupRepositoryTemplateDualKey<TEntityType, TKeyType1, TKeyType2>
    where TKeyType1 : struct where TKeyType2 : struct
{
}
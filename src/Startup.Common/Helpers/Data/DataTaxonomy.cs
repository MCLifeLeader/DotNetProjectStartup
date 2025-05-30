﻿using Microsoft.Extensions.Compliance.Classification;
using System.Diagnostics.CodeAnalysis;

namespace Startup.Common.Helpers.Data;

/// <summary>
/// You may run into trouble with the Logger autogen if you put this in a core class
/// </summary>
[ExcludeFromCodeCoverage]
public static class DataTaxonomy
{
    public static string TaxonomyName { get; } = typeof(DataTaxonomy).FullName!;

    public static DataClassification SensitiveData { get; } = new(TaxonomyName, nameof(SensitiveData));
    public static DataClassification PartialSensitiveData { get; } = new(TaxonomyName, nameof(PartialSensitiveData));
    public static DataClassification Pii { get; } = new(TaxonomyName, nameof(Pii));
}
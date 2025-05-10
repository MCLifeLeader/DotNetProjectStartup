using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Startup.Common.Helpers;

/// <summary>
/// Helper functions for supporting additional operators with Guid objects.
/// https://stackoverflow.com/questions/54920200/entity-framework-core-guid-greater-than-for-paging
/// </summary>
public static class GuidFunctions
{
    /// <summary>
    /// Determines whether the left Guid is greater than the right Guid.
    /// </summary>
    /// <param name="left">The left Guid.</param>
    /// <param name="right">The right Guid.</param>
    /// <returns>True if the left Guid is greater than the right Guid; otherwise, false.</returns>
    public static bool IsGreaterThan(this Guid left, Guid right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Determines whether the left Guid is greater than or equal to the right Guid.
    /// </summary>
    /// <param name="left">The left Guid.</param>
    /// <param name="right">The right Guid.</param>
    /// <returns>True if the left Guid is greater than or equal to the right Guid; otherwise, false.</returns>
    public static bool IsGreaterThanOrEqual(this Guid left, Guid right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Determines whether the left Guid is less than the right Guid.
    /// </summary>
    /// <param name="left">The left Guid.</param>
    /// <param name="right">The right Guid.</param>
    /// <returns>True if the left Guid is less than the right Guid; otherwise, false.</returns>
    public static bool IsLessThan(this Guid left, Guid right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines whether the left Guid is less than or equal to the right Guid.
    /// </summary>
    /// <param name="left">The left Guid.</param>
    /// <param name="right">The right Guid.</param>
    /// <returns>True if the left Guid is less than or equal to the right Guid; otherwise, false.</returns>
    public static bool IsLessThanOrEqual(this Guid left, Guid right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Registers the Guid comparison functions with the specified model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder to register the functions with.</param>
    public static void Register(ModelBuilder modelBuilder)
    {
        RegisterFunction(modelBuilder, nameof(IsGreaterThan), ExpressionType.GreaterThan);
        RegisterFunction(modelBuilder, nameof(IsGreaterThanOrEqual), ExpressionType.GreaterThanOrEqual);
        RegisterFunction(modelBuilder, nameof(IsLessThan), ExpressionType.LessThan);
        RegisterFunction(modelBuilder, nameof(IsLessThanOrEqual), ExpressionType.LessThanOrEqual);
    }

    /// <summary>
    /// Registers a specific Guid comparison function with the specified model builder.
    /// </summary>
    /// <param name="modelBuilder">The model builder to register the function with.</param>
    /// <param name="name">The name of the function to register.</param>
    /// <param name="type">The expression type of the function.</param>
    static void RegisterFunction(ModelBuilder modelBuilder, string name, ExpressionType type)
    {
        MethodInfo? method = typeof(GuidFunctions).GetMethod(name, new[]
        {
            typeof(Guid),
            typeof(Guid)
        });
        if (method != null)
        {
            modelBuilder.HasDbFunction(method).HasTranslation(parameters =>
            {
                SqlExpression left = parameters.ElementAt(0);
                SqlExpression right = parameters.ElementAt(1);

                return new SqlBinaryExpression(type, left, right, typeof(bool), null);
            });
        }
    }
}

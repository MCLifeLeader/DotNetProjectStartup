namespace Startup.Common.Repositories.Interfaces;

/// <summary>
///     Base interface context used to bind all of the database repositories into a unit of work
/// </summary>
public interface IDbContextBase
{
    int SaveChanges();

    Task<int> SaveChangesAsync();

    /// <summary>
    ///     Reloads the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity.</param>
    void Reload<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    ///     Clears the state.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity.</param>
    void ClearState<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    ///     Sets the database context configuration automatic detect changes.
    /// </summary>
    /// <param name="setAutoDetect">if set to <c>true</c> [set automatic detect].</param>
    void SetDbContextConfigurationAutoDetectChanges(bool setAutoDetect);
}
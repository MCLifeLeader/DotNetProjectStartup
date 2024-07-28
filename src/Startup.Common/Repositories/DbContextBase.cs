using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Startup.Common.Helpers;
using Startup.Common.Helpers.Extensions;
using Startup.Common.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Startup.Common.Repositories;

public class DbContextBase<TDbContext> : DbContext, IDbContextBase where TDbContext : DbContext
{
    private readonly string _connectionString;

#pragma warning disable EF1001 // Internal EF Core API usage.
    protected DbContextBase(DbContextOptions options) : base(new DbContextOptionsBuilder<TDbContext>()
            .UseSqlServer(
            options.FindExtension<SqlServerOptionsExtension>()?.ConnectionString ?? string.Empty).Options)
    {
        // The dependency injection resolver keeps sending requests through this constructor. This bit of code in the base ctor
        // redirects the generic template constructor below with the signature of "DbContextOptions<TDbContext> options"

        // ToDo: This has code smell to it, see if we can find a more elegant solution.
    }
#pragma warning restore EF1001 // Internal EF Core API usage.

    protected DbContextBase(DbContextOptions<TDbContext> options) : base(options)
    {
    }

    protected DbContextBase(DbContextOptionsBuilder<TDbContext> builder) : base(builder.Options)
    {
        OnConfiguring(builder);
    }

    protected DbContextBase(string connectionString, DbContextOptions<TDbContext> options) : base(options)
    {
        _connectionString = connectionString;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        GuidFunctions.Register(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_connectionString != null)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        if (Debugger.IsAttached)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseLoggerFactory(LoggerSupport.GetLoggerFactory(null));
        }

        base.OnConfiguring(optionsBuilder);
    }

    #region Public Methods

    public async Task<int> SaveChangesAsync()
    {
        Validate();
        return await base.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        Validate();
        return base.SaveChanges();
    }

    /// <summary>
    ///     Reloads the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity.</param>
    public void Reload<TEntity>(TEntity entity) where TEntity : class
    {
        try
        {
            Entry(entity).Reload();
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     Clears the state.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity.</param>
    public void ClearState<TEntity>(TEntity entity) where TEntity : class
    {
        try
        {
            Entry(entity).CurrentValues.SetValues(Entry(entity).OriginalValues);
        }
        catch
        {
            // ignored
        }

        try
        {
            Entry(entity).Reload();
        }
        catch
        {
            // ignored
        }

        try
        {
            Entry(entity).State = EntityState.Unchanged;
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     Sets the database context configuration automatic detect changes.
    /// </summary>
    /// <param name="setAutoDetect">if set to <c>true</c> [set automatic detect].</param>
    public void SetDbContextConfigurationAutoDetectChanges(bool setAutoDetect)
    {
        ChangeTracker.AutoDetectChangesEnabled = setAutoDetect;
    }

    protected static DbContextOptions<TDbContext> GetOptionsGeneric(string connectionString)
    {
        // ReSharper disable once InvokeAsExtensionMethod
        return new DbContextOptionsBuilder<TDbContext>().UseSqlServer(connectionString).Options;
    }

    #endregion

    #region Private Methods

    private void Validate()
    {
        IEnumerable<object> entities = from e in ChangeTracker.Entries()
            where e.State == EntityState.Added
                  || e.State == EntityState.Modified
            select e.Entity;

        foreach (object entity in entities)
        {
            ValidationContext validationContext = new ValidationContext(entity);
            Validator.ValidateObject(entity, validationContext);
        }
    }

    #endregion
}
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Api.Startup.Example.Helpers;
using Api.Startup.Example.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Api.Startup.Example.Repositories;

public class DbContextBase<TDbContext> : DbContext, IDbContextBase
    where TDbContext : DbContext
{
    private readonly string _connectionString;
    private readonly bool _enableDebuggingSensitiveDataLogging;

#pragma warning disable EF1001 // Internal EF Core API usage.
    protected DbContextBase(DbContextOptions options) : base(
        new DbContextOptionsBuilder<TDbContext>().UseSqlServer(
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
    }

    protected DbContextBase(
        string connectionString,
        DbContextOptions<TDbContext> options,
        bool enableDebuggingSensitiveDataLogging = false) : base(options)
    {
        _connectionString = connectionString;
        _enableDebuggingSensitiveDataLogging = enableDebuggingSensitiveDataLogging;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        GuidFunctions.Register(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_connectionString != null) optionsBuilder.UseSqlServer(_connectionString);

        if (Debugger.IsAttached && _enableDebuggingSensitiveDataLogging)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
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

    private ILoggerFactory GetLoggerFactory()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder =>
        {
            builder
                .AddConsole()
                .AddDebug()
                .AddFilter(DbLoggerCategory.Database.Command.Name,
                    LogLevel.Information)
                .AddFilter(DbLoggerCategory.Query.Name,
                    LogLevel.Information)
                .AddFilter(DbLoggerCategory.Update.Name,
                    LogLevel.Information);
        });
        return serviceCollection.BuildServiceProvider()
            .GetService<ILoggerFactory>();
    }

    #endregion
}
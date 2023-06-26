using Api.Startup.Example.Repositories.Interfaces;

namespace Api.Startup.Example.Repositories.Db.Interfaces;

/// <summary>
///     Base interface context used to bind all of the database repositories into a unit of work
/// </summary>
public interface IStartupExampleContext : IDbContextBase
{
}
using React.Startup.Example.Repositories.Interfaces;

namespace React.Startup.Example.Repositories.Db.Interfaces;

/// <summary>
///     Base interface context used to bind all of the database repositories into a unit of work
/// </summary>
public interface IStartupExampleContext : IDbContextBase
{
}
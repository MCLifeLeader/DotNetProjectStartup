using Startup.Common.Repositories.Interfaces;

namespace Startup.Data.Repositories.Db.Interfaces;

/// <summary>
///     Base interface context used to bind all of the database repositories into a unit of work
/// </summary>
public interface IStartupExampleContext : IDbContextBase
{
}
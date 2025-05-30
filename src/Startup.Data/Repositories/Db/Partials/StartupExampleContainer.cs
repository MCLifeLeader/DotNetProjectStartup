#nullable disable
using Microsoft.EntityFrameworkCore;
using Startup.Common.Repositories;
using Startup.Data.Repositories.Db.Interfaces;

// ReSharper disable All
namespace Startup.Data.Repositories.Db;

/// <summary>
///     This context should be kept in favor of the auto generated code.
///     In the auto generated code for ..\StartupExampleContainer.cs remove the ": DbContext" reference and the generated
///     constructors.
/// </summary>
public partial class StartupExampleContainer : DbContextBase<StartupExampleContainer>, IStartupExampleContext
{
    public StartupExampleContainer(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }
}
// ReSharper restore All
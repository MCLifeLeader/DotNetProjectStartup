using Microsoft.EntityFrameworkCore;
using StartupExample.Data.Repositories.Db;

namespace Api.Startup.Example.Repositories.Db;

public class StartupExampleContext : StartupExampleContainer
{
    public StartupExampleContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public StartupExampleContext(DbContextOptions<StartupExampleContainer> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public StartupExampleContext(string connectionString)
        : base(GetOptionsGeneric(connectionString))
    {
    }
}
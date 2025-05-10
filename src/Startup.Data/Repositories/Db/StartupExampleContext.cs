using Microsoft.EntityFrameworkCore;

namespace Startup.Data.Repositories.Db;

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
}
using Api.Startup.Example.Repositories.Db.Interfaces;

namespace Api.Startup.Example.Repositories.Db;

public abstract class StartupExampleRepositoryBase : RepositoryBase<StartupExampleContext>
{
    protected StartupExampleRepositoryBase(IStartupExampleContext dbContext) : base(dbContext)
    {
    }
}
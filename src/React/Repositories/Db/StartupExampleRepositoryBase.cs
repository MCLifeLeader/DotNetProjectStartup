using React.Startup.Example.Repositories.Db.Interfaces;

namespace React.Startup.Example.Repositories.Db;

public abstract class StartupExampleRepositoryBase : RepositoryBase<StartupExampleContext>
{
    protected StartupExampleRepositoryBase(IStartupExampleContext dbContext) : base(dbContext)
    {
    }
}
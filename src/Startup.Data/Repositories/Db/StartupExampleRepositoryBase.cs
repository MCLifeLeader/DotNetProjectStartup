using Startup.Common.Repositories;
using Startup.Data.Repositories.Db.Interfaces;

namespace Startup.Data.Repositories.Db;

public abstract class StartupExampleRepositoryBase : RepositoryBase<StartupExampleContext>
{
    protected StartupExampleRepositoryBase(IStartupExampleContext dbContext) : base(dbContext)
    {
    }
}
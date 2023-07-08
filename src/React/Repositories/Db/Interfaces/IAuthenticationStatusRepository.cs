using React.Startup.Example.Repositories.Interfaces;
using StartupExample.Data.Model.Db;

namespace React.Startup.Example.Repositories.Db.Interfaces;

public interface IAuthenticationStatusRepository : ILookupRepository<AuthenticationStatus, short>, IDisposable
{
}
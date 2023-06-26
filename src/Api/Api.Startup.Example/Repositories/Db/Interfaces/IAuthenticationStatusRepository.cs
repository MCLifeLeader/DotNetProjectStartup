using Api.Startup.Example.Repositories.Interfaces;
using StartupExample.Data.Model.Db;

namespace Api.Startup.Example.Repositories.Db.Interfaces;

public interface IAuthenticationStatusRepository : ILookupRepository<AuthenticationStatus, short>, IDisposable
{
}
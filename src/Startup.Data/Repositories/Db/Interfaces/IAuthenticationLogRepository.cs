using Startup.Data.Models.Db;
using Startup.Data.Repositories.Interfaces;

namespace Startup.Data.Repositories.Db.Interfaces;

public interface IAuthenticationLogRepository : IRepository<AuthenticationLog, Guid>, IDisposable
{
}
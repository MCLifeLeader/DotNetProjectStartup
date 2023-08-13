using Startup.Data.Models.Db;
using Startup.Data.Repositories.Interfaces;

namespace Startup.Data.Repositories.Db.Interfaces;

public interface IAgencyRepository : IRepository<Agency, Guid>, IDisposable
{
}
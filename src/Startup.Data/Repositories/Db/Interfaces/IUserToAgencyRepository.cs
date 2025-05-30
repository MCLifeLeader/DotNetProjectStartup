using Startup.Common.Repositories.Interfaces;
using Startup.Data.Models.Db.dboSchema;

namespace Startup.Data.Repositories.Db.Interfaces;

public interface IUserToAgencyRepository : INonStructRepositoryDualKey<UserToAgency, string, Guid>, IDisposable
{
}
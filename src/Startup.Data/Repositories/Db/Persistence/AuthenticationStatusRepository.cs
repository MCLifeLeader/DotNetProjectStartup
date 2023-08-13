using Microsoft.EntityFrameworkCore;
using Startup.Data.Models;
using Startup.Data.Models.Db;
using Startup.Data.Repositories.Db.Interfaces;
using System.Linq.Expressions;

namespace Startup.Data.Repositories.Db.Persistence;

public class AuthenticationStatusRepository : StartupExampleRepositoryBase, IAuthenticationStatusRepository
{
    public AuthenticationStatusRepository(IStartupExampleContext dbContext) : base(dbContext)
    {
    }

    public AuthenticationStatus GetEntityById(short key)
    {
        return _context.AuthenticationStatuses.SingleOrDefault(x => x.Id == key);
    }

    public async Task<AuthenticationStatus> GetEntityByIdAsync(short key)
    {
        return await _context.AuthenticationStatuses.SingleOrDefaultAsync(x => x.Id == key);
    }

    public IList<AuthenticationStatus> GetAll()
    {
        return _context.AuthenticationStatuses.ToList();
    }

    public async Task<IList<AuthenticationStatus>> GetAllAsync()
    {
        return await _context.AuthenticationStatuses.ToListAsync();
    }

    public PagedObjectData<AuthenticationStatus> GetByPaging(short key, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedObjectData<AuthenticationStatus>> GetByPagingAsync(short key, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IQueryable<AuthenticationStatus> Query(Expression<Func<AuthenticationStatus, bool>> filter)
    {
        return _context.AuthenticationStatuses.Where(filter);
    }

    public IQueryable<AuthenticationStatus> GetAsQueryable()
    {
        return _context.AuthenticationStatuses.AsQueryable();
    }
}
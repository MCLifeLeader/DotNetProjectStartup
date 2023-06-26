using System.Linq.Expressions;
using Api.Startup.Example.Repositories.Db.Interfaces;
using Microsoft.EntityFrameworkCore;
using StartupExample.Data.Model.Db;

namespace Api.Startup.Example.Repositories.Db.Persistence;

public class UserToAgencyRepository : StartupExampleRepositoryBase, IUserToAgencyRepository
{
    public UserToAgencyRepository(IStartupExampleContext dbContext) : base(dbContext)
    {
    }

    public void Add(UserToAgency entity)
    {
        _context.UserToAgencies.Add(entity);
    }

    public async Task AddAsync(UserToAgency entity)
    {
        await _context.UserToAgencies.AddAsync(entity);
    }

    public void AddRange(IList<UserToAgency> entities)
    {
        _context.UserToAgencies.AddRange(entities);
    }

    public async Task AddRangeAsync(IList<UserToAgency> entities)
    {
        await _context.UserToAgencies.AddRangeAsync(entities);
    }

    public void Delete(UserToAgency entity)
    {
        _context.UserToAgencies.Remove(entity);
    }

    public void Update(UserToAgency entity)
    {
        _context.UserToAgencies.Update(entity);
    }

    public void UpdateRange(IList<UserToAgency> entities)
    {
        _context.UserToAgencies.UpdateRange(entities);
    }

    public UserToAgency GetEntityById(string key1, Guid key2)
    {
        return _context.UserToAgencies.SingleOrDefault(e => e.UserId.Equals(key2) && e.AgencyId == key2);
    }

    public IList<UserToAgency> GetEntityByFirstKeyId(string key1)
    {
        return _context.UserToAgencies.Where(e => e.UserId.Equals(key1)).ToList();
    }

    public IList<UserToAgency> GetEntityBySecondKeyId(Guid key2)
    {
        return _context.UserToAgencies.Where(e => e.AgencyId == key2).ToList();
    }

    public async Task<UserToAgency> GetEntityByIdAsync(string key1, Guid key2)
    {
        return await _context.UserToAgencies.SingleOrDefaultAsync(e => e.UserId.Equals(key2) && e.AgencyId == key2);
    }

    public async Task<IList<UserToAgency>> GetEntityByFirstKeyIdAsync(string key1)
    {
        return await _context.UserToAgencies.Where(e => e.UserId.Equals(key1)).ToListAsync();
    }

    public async Task<IList<UserToAgency>> GetEntityBySecondKeyIdAsync(Guid key2)
    {
        return await _context.UserToAgencies.Where(e => e.AgencyId == key2).ToListAsync();
    }

    public IList<UserToAgency> GetAll()
    {
        return _context.UserToAgencies.ToList();
    }

    public async Task<IList<UserToAgency>> GetAllAsync()
    {
        return await _context.UserToAgencies.ToListAsync();
    }

    public IQueryable<UserToAgency> Query(Expression<Func<UserToAgency, bool>> filter)
    {
        return _context.UserToAgencies.Where(filter);
    }

    public IQueryable<UserToAgency> GetAsQueryable()
    {
        return _context.UserToAgencies.AsQueryable();
    }
}
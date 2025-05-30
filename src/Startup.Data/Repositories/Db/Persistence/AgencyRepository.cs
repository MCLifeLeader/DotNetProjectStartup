using Microsoft.EntityFrameworkCore;
using Startup.Common.Helpers;
using Startup.Common.Models;
using Startup.Data.Models.Db.dboSchema;
using Startup.Data.Repositories.Db.Interfaces;
using System.Linq.Expressions;

namespace Startup.Data.Repositories.Db.Persistence;

public class AgencyRepository : StartupExampleRepositoryBase, IAgencyRepository
{
    public AgencyRepository(IStartupExampleContext dbContext) : base(dbContext)
    {
    }

    public void Add(Agency entity)
    {
        _context.Agencies.Add(entity);
    }

    public async Task AddAsync(Agency entity)
    {
        await _context.Agencies.AddAsync(entity);
    }

    public void AddRange(IList<Agency> entities)
    {
        _context.Agencies.AddRange(entities);
    }

    public async Task AddRangeAsync(IList<Agency> entities)
    {
        await _context.Agencies.AddRangeAsync(entities);
    }

    public void Delete(Agency entity)
    {
        _context.Agencies.Remove(entity);
    }

    public void Update(Agency entity)
    {
        entity.LastUpdated = DateTime.UtcNow;

        _context.Agencies.Update(entity);
    }

    public void UpdateRange(IList<Agency> entities)
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int ii = 0; ii < entities.Count; ii++)
        {
            entities[ii].LastUpdated = DateTime.UtcNow;
        }

        _context.Agencies.UpdateRange(entities);
    }

    public Agency GetEntityById(Guid key)
    {
        return _context.Agencies.SingleOrDefault(e => e.Id == key);
    }

    public async Task<Agency> GetEntityByIdAsync(Guid key)
    {
        return await _context.Agencies.SingleOrDefaultAsync(e => e.Id == key);
    }

    public IList<Agency> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Agency>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public IQueryable<Agency> Query(Expression<Func<Agency, bool>> filter)
    {
        return _context.Agencies.Where(filter);
    }

    public IQueryable<Agency> GetAsQueryable()
    {
        return _context.Agencies.AsQueryable();
    }

    public Common.Models.ChunkedObjectData<Agency> GetByChunking(Guid key1, int pageSize)
    {
        long totalRecords = _context.Agencies.Count();

        List<Agency> entityList = _context.Agencies
            .OrderBy(o => o.Id)
            .Where(e => e.Id.IsGreaterThan(key1))
            .Take(pageSize)
            .ToList();

        ChunkedObjectData<Agency> pagedObjectData = new()
        {
            EntityList = entityList,
            TotalItemCount = totalRecords,
            PageSize = pageSize
        };

        return pagedObjectData;
    }

    public async Task<Common.Models.ChunkedObjectData<Agency>> GetByChunkingAsync(Guid key1, int pageSize)
    {
        long totalRecords = await _context.Agencies.CountAsync();

        List<Agency> entityList = await _context.Agencies
        .OrderBy(o => o.Id)
            .Where(e => e.Id.IsGreaterThan(key1))
            .Take(pageSize)
            .ToListAsync();

        ChunkedObjectData<Agency> pagedObjectData = new()
        {
            EntityList = entityList,
            TotalItemCount = totalRecords,
            PageSize = pageSize
        };

        return pagedObjectData;
    }

    public Common.Models.PagedObjectData<Agency> GetByPaging(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Common.Models.PagedObjectData<Agency>> GetByPagingAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
}
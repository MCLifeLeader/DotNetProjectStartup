﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using React.Startup.Example.Helpers;
using React.Startup.Example.Models;
using React.Startup.Example.Repositories.Db.Interfaces;
using StartupExample.Data.Model.Db;

namespace React.Startup.Example.Repositories.Db.Persistence;

public class AuthenticationLogRepository : StartupExampleRepositoryBase, IAuthenticationLogRepository
{
    public AuthenticationLogRepository(IStartupExampleContext dbContext) : base(dbContext)
    {
    }

    public void Add(AuthenticationLog entity)
    {
        _context.AuthenticationLogs.Add(entity);
    }

    public async Task AddAsync(AuthenticationLog entity)
    {
        await _context.AuthenticationLogs.AddAsync(entity);
    }

    public void AddRange(IList<AuthenticationLog> entities)
    {
        _context.AuthenticationLogs.AddRange(entities);
    }

    public async Task AddRangeAsync(IList<AuthenticationLog> entities)
    {
        await _context.AuthenticationLogs.AddRangeAsync(entities);
    }

    public void Delete(AuthenticationLog entity)
    {
        throw new NotImplementedException();
    }

    public void Update(AuthenticationLog entity)
    {
        throw new NotImplementedException();
    }

    public void UpdateRange(IList<AuthenticationLog> entities)
    {
        throw new NotImplementedException();
    }

    public AuthenticationLog GetEntityById(Guid key)
    {
        return _context.AuthenticationLogs.SingleOrDefault(e => e.Id == key);
    }

    public async Task<AuthenticationLog> GetEntityByIdAsync(Guid key)
    {
        return await _context.AuthenticationLogs.SingleOrDefaultAsync(e => e.Id == key);
    }

    public IList<AuthenticationLog> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<IList<AuthenticationLog>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public PagedObjectData<AuthenticationLog> GetByPaging(Guid key, int pageSize)
    {
        long totalRecords = _context.AuthenticationLogs.Count();

        List<AuthenticationLog> entityList = _context.AuthenticationLogs
            .OrderBy(o => o.Id)
            .Where(e => e.Id.IsGreaterThan(key))
            .Take(pageSize)
            .ToList();

        PagedObjectData<AuthenticationLog> pagedObjectData = new()
        {
            EntityList = entityList,
            TotalRecordsInTable = totalRecords,
            PageSize = pageSize
        };

        return pagedObjectData;
    }

    public async Task<PagedObjectData<AuthenticationLog>> GetByPagingAsync(Guid key, int pageSize)
    {
        long totalRecords = await _context.AuthenticationLogs.CountAsync();

        List<AuthenticationLog> entityList = await _context.AuthenticationLogs
            .OrderBy(o => o.Id)
            .Where(e => e.Id.IsGreaterThan(key))
            .Take(pageSize)
            .ToListAsync();

        PagedObjectData<AuthenticationLog> pagedObjectData = new()
        {
            EntityList = entityList,
            TotalRecordsInTable = totalRecords,
            PageSize = pageSize
        };

        return pagedObjectData;
    }

    public IQueryable<AuthenticationLog> Query(Expression<Func<AuthenticationLog, bool>> filter)
    {
        return _context.AuthenticationLogs.Where(filter);
    }

    public IQueryable<AuthenticationLog> GetAsQueryable()
    {
        return _context.AuthenticationLogs.AsQueryable();
    }
}
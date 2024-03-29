﻿using System.Collections;
using Infinite.Core.Interfaces.Repositories;
using Infinite.Core.Persistence;
using Infinite.Shared.Contracts;

namespace Infinite.Core.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public AppDbContext AppDbContext { get; }
    private bool _disposed;
    private Hashtable _repositories;

    public UnitOfWork(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }

    public IRepositoryAsync<T> GetRepository<T>() where T : class, IEntity
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryAsync<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), AppDbContext);
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepositoryAsync<T>)_repositories[type];
    }

    public async Task<int> Commit()
    {
        return await AppDbContext.SaveChangesAsync();
    }

    public Task Rollback()
    {
        AppDbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                AppDbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}

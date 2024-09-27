﻿using Infrastructure.DAL.Context;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common.Attribute;

namespace Infrastructure.DAL.UnitOfWork;

public interface IUnitOfWork: IDisposable
{
    public bool SaveChanges();
    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    public void Commit();
    public Task CommitAsync(CancellationToken cancellationToken = default);

    public void Rollback();

    public Task RollbackAsync(CancellationToken cancellationToken = default);
}


[Injectable(InterfaceType = typeof(IUnitOfWork), Lifetime = ServiceLifetime.Scoped)]
public sealed class UnitOfWork: IUnitOfWork
{
    private ApplicationDbContext Context { get; }
    
    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        Context = applicationDbContext;
    }

    public bool SaveChanges()
    {
        return Context.SaveChanges() > 0;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Commit()
    {
        Context.Database.CommitTransaction();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return Context.Database.CommitTransactionAsync(cancellationToken);
    }
    
    public void Rollback()
    {
        Context.Database.RollbackTransaction();
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return Context.Database.RollbackTransactionAsync(cancellationToken);
    }
    

    
    private bool _disposed = false;
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //TODO:
                // Implement context dispose here
                Context.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
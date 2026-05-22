using EventTicket.Core.Entities;
using EventTicket.Core.Interfaces;
using EventTicket.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventTicket.Data.Repositories.Abstract;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    protected RepositoryBase(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await DbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await DbSet.ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await DbSet.Where(predicate).ToListAsync();

    public virtual async Task<IEnumerable<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;
        foreach (var include in includes)
            query = query.Include(include);
        return await query.Where(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindIgnoreFiltersAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet.IgnoreQueryFilters();
        foreach (var include in includes)
            query = query.Include(include);
        return await query.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;
        foreach (var include in includes)
            query = query.Include(include);
        return await query.FirstOrDefaultAsync(predicate);
    }


    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await DbSet.AnyAsync(predicate);

   
    public virtual async Task<IEnumerable<T>> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = predicate != null ? DbSet.Where(predicate) : DbSet;
        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }


    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;

        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            baseEntity.DeletedAt = DateTime.UtcNow;
            DbSet.Update(entity);
        }
        else
        {
            DbSet.Remove(entity);
        }

        await Context.SaveChangesAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        => predicate is null
            ? await DbSet.CountAsync()
            : await DbSet.CountAsync(predicate);
}
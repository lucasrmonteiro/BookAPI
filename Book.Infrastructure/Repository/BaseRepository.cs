using Book.Core.Base;
using Book.Core.Repository.Base;
using Book.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Book.Infrastructure.Repository;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly BookDbContext _dataContext;
    public BaseRepository(BookDbContext dataContext)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(BookDbContext));
    }
    
    public Task<List<TEntity>> GetAllAsync()
        => _dataContext.Set<TEntity>().ToListAsync();
   
    public Task<TEntity?> GetByIdAsync(Guid id)
        => _dataContext.Set<TEntity>()
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _dataContext.Set<TEntity>().Add(entity);
        await _dataContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dataContext.Entry(entity).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dataContext.Set<TEntity>().Remove(entity);
        await _dataContext.SaveChangesAsync();
    }
}
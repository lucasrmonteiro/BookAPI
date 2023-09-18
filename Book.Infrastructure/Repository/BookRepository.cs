using Book.Core.Repository;
using Book.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Book.Infrastructure.Repository;

public class BookRepository : BaseRepository<Core.Entities.Book> , IBookRepository
{
    public BookRepository(BookDbContext dataContext) : base(dataContext)
    {
    }
    
    public async Task<Core.Entities.Book?> GetByIdWithCategory(Guid id)
        => await _dataContext.Books
            .Include(x => x.Category)
            .SingleOrDefaultAsync(x => x.Id == id);
    
    public async Task<IEnumerable<Core.Entities.Book>> GetAllWithCategory()
         => await _dataContext.Books
            .Include(x => x.Category)
            .ToListAsync();
}
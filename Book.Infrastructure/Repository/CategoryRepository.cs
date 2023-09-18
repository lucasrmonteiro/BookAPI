using Book.Core.Repository;
using Book.Infrastructure.Data;

namespace Book.Infrastructure.Repository;

public class CategoryRepository : BaseRepository<Core.Entities.Category> , ICategoryRepository
{
    public CategoryRepository(BookDbContext dataContext) : base(dataContext)
    {
    }
}
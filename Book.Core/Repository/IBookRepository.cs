using Book.Core.Base;
using Book.Core.Repository.Base;

namespace Book.Core.Repository;

public interface IBookRepository : IBaseRepository<Entities.Book>
{
    Task<Entities.Book?> GetByIdWithCategory(Guid id);
    Task<IEnumerable<Core.Entities.Book>> GetAllWithCategory();
}
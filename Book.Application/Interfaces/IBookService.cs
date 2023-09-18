using Book.Application.Models;

namespace Book.Application.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookModel>> ListAllBooks();
    Task<BookModel?> GetBookById(Guid id);
    Task<BookModel?> Create(BookModel book);
    Task Update(BookModel book);
    Task Delete(Guid id);
}
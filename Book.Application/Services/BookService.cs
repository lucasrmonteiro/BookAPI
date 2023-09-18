using Book.Application.Interfaces;
using Book.Application.Models;
using Book.Core.Entities;
using Book.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Book.Application.Services;

public class BookService : IBookService
{
    private readonly ILogger<BookService> _logger;
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public BookService(IBookRepository bookRepository
        , ICategoryRepository categoryRepository
        , ILogger<BookService> logger)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IEnumerable<BookModel>> ListAllBooks()
    {
        var data = await _bookRepository.GetAllWithCategory();
        
        if(data.Any())
            return data.Select(PopulateModel);
        else
        {
            _logger.LogWarning("No data found");
            return new List<BookModel>();
        }
    }

    public async Task<BookModel?> GetBookById(Guid id)
    {
        var data = await _bookRepository.GetByIdWithCategory(id);
        
        if (data is null)
            return null;
        
        return PopulateModel(data);
    }
       
    
    public async Task<BookModel?> Create(BookModel book)
    {
        var category = await _categoryRepository.GetByIdAsync(book.Category.CategoryId);

        if (category is null)
            throw new ArgumentException(typeof(Category).ToString());
        
        var entity = new Core.Entities.Book()
        {
            Title = book.Title,
            Author = book.Author,
            Category = category
        };
        
        var data = await _bookRepository.AddAsync(entity);
        
        return PopulateModel(data);
    }
    
    public async Task Update(BookModel book)
    {
        var bookEntity = await _bookRepository.GetByIdAsync(book.BookId.Value);
        var categoryData = await _categoryRepository.GetByIdAsync(book.Category.CategoryId);

        if (bookEntity is null)
            throw new ArgumentException(typeof(Core.Entities.Book).ToString());
        
        if (categoryData is null)
            throw new ArgumentException(typeof(Core.Entities.Book).ToString());
        
        bookEntity.Title = book.Title;
        bookEntity.Author = book.Author;
        bookEntity.Category = categoryData;
        
        await _bookRepository.UpdateAsync(bookEntity);
    }
    
    public async Task Delete(Guid id)
    {
        var bookData = await _bookRepository.GetByIdAsync(id);

        if (bookData is null)
            throw new ArgumentException(typeof(Core.Entities.Book).ToString());
        
        await _bookRepository.DeleteAsync(bookData);
    }
    
    private static BookModel? PopulateModel(Core.Entities.Book bookData)
    {
        return  new BookModel(bookData.Id ,bookData.Title, bookData.Author, new CategoryModel(bookData.Category.Id ,bookData.Category.Name));
    }
}
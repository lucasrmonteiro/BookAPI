using Book.Application.Models;
using Book.Application.Services;
using Book.Core.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace Book.Application.Tests;

public class BookServiceTests
{
    [Fact]
    public async Task ListAllBooks_ReturnsEmptyList_WhenNoData()
    {
        // Arrange
        var bookRepositoryMock = new Mock<IBookRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        bookRepositoryMock.Setup(repo => repo.GetAllWithCategory()).ReturnsAsync(new List<Core.Entities.Book>());

        var bookService = new BookService(bookRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object);

        // Act
        var result = await bookService.ListAllBooks();

        // Assert
        Assert.IsEmpty(result);
    }

    [Fact]
    public async Task GetBookById_ReturnsNull_WhenBookNotFound()
    {
        // Arrange
        var bookRepositoryMock = new Mock<IBookRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        Guid nonExistentBookId = Guid.NewGuid();
        bookRepositoryMock.Setup(repo => repo.GetByIdWithCategory(nonExistentBookId)).ReturnsAsync((Core.Entities.Book)null);

        var bookService = new BookService(bookRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object);

        // Act
        var result = await bookService.GetBookById(nonExistentBookId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ThrowsArgumentException_WhenCategoryNotFound()
    {
        // Arrange
        var bookRepositoryMock = new Mock<IBookRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        // Configure the category repository mock to return null for the category
        categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Core.Entities.Category)null);

        var bookService = new BookService(bookRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object);
        var bookModel = new BookModel(null , "Title", "Author" , new CategoryModel(new Guid() ,"teste"));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => bookService.Create(bookModel));
    }
    
        [Fact]
    public async Task Create_SuccessfullyCreatesBook()
    {
        // Arrange
        var bookRepositoryMock = new Mock<IBookRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();
        var category = new Core.Entities.Category()
        {
            Id = new Guid(),
            Name = "Fantasy"
        };
        
        categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(category);

        var bookService = new BookService(bookRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object);
        var bookModel = new BookModel(null , "Title", "Author" , new CategoryModel(category.Id ,category.Name));
        
        bookRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Core.Entities.Book>())).ReturnsAsync(new Core.Entities.Book
        {
            Id = Guid.NewGuid(), 
            Title = bookModel.Title,
            Author = bookModel.Author,
            Category = category
        });

        // Act
        var result = await bookService.Create(bookModel);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Title, Is.EqualTo(bookModel.Title));
        Assert.That(result.Author, Is.EqualTo(bookModel.Author));
        // Add more assertions as needed
    }

    [Fact]
    public async Task Update_SuccessfullyUpdatesBook()
    {
        // Arrange
        var bookRepositoryMock = new Mock<IBookRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var existingBookId = Guid.NewGuid();
        var existingCategory = new Core.Entities.Category()
        {
            Id = new Guid(),
            Name = "Fantasy"
        };
        var bookModel = new BookModel(existingBookId , "Updated Title", "Updated Author" , new CategoryModel(existingCategory.Id ,existingCategory.Name));
        
        bookRepositoryMock.Setup(repo => repo.GetByIdAsync(existingBookId)).ReturnsAsync(new Core.Entities.Book
        {
            Id = existingBookId,
            Title = "Old Title",
            Author = "Old Author",
            Category = existingCategory
        });
        
        categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(existingCategory.Id)).ReturnsAsync(existingCategory);

        var bookService = new BookService(bookRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object);

        // Act
        await bookService.Update(bookModel);

        // Assert
        bookRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Core.Entities.Book>()), Times.Once);
    }
    
    [Fact]
    public async Task Delete_SuccessfullyDeletesBook()
    {
        // Arrange
        var bookRepositoryMock = new Mock<IBookRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var loggerMock = new Mock<ILogger<BookService>>();

        var existingBookId = Guid.NewGuid();
        var existingCategory = new Core.Entities.Category()
        {
            Id = new Guid(),
            Name = "Fantasy"
        };
        
        bookRepositoryMock.Setup(repo => repo.GetByIdAsync(existingBookId)).ReturnsAsync(new Core.Entities.Book
        {
            Id = existingBookId,
            Title = "Existing Book",
            Author = "Author",
            Category = existingCategory
        });

        var bookService = new BookService(bookRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object);

        // Act
        await bookService.Delete(existingBookId);

        // Assert
        bookRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Core.Entities.Book>()), Times.Once);
    }
}
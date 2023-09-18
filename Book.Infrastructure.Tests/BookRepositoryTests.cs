using System.Linq.Expressions;
using Book.Infrastructure.Data;
using Book.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace Book.Infrastructure.Tests;

public class BookRepositoryTests
{
    [Fact]
    public async Task GetByIdWithCategory_Returns_Entity_IfExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var dbContext = new BookDbContext(options);

        var entityId = Guid.NewGuid();
        var category = new Core.Entities.Category()
        {
            Id = new Guid(),
            Name = "Fantasy"
        };
        var book = new Core.Entities.Book
        {
            Id = entityId, Category = category ,
            Title = "Name of the Wind",
            Author = "Patrick Rothfuss",
        };
        dbContext.Add(book);
        dbContext.SaveChanges();

        var repository = new BookRepository(dbContext);

        // Act
        var result = await repository.GetByIdWithCategory(entityId);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Id, Is.EqualTo(entityId));
    }

    [Fact]
    public async Task GetByIdWithCategory_Returns_Null_If_Entity_Does_Not_Exist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var dbContext = new BookDbContext(options);

        var repository = new BookRepository(dbContext);

        // Act
        var result = await repository.GetByIdWithCategory(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllWithCategory_Returns_AllEntities()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var dbContext = new BookDbContext(options);
        var category = new Core.Entities.Category()
        {
            Id = new Guid(),
            Name = "Fantasy"
        };

        var books = new List<Core.Entities.Book>
        {
            new Core.Entities.Book
            {
                Id = Guid.NewGuid(), Category = category ,
                Title = "Name of the Wind",
                Author = "Patrick Rothfuss",
            },
            new Core.Entities.Book
            {
                Id = Guid.NewGuid(), Category = category,
                Title = "title 1",
                Author = "Author 1",
            },
            new Core.Entities.Book
            {
                Id = Guid.NewGuid(), Category = category,
                Title = "title 2",
                Author = "Author 2",
            }
        };

        dbContext.AddRange(books);
        dbContext.SaveChanges();

        var repository = new BookRepository(dbContext);

        // Act
        var result = await repository.GetAllWithCategory();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(books.Count));
    }
}
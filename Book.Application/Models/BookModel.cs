namespace Book.Application.Models;

public record BookModel(Guid? BookId, string Title, string Author, CategoryModel Category);
using Book.Core.Base;

namespace Book.Core.Entities;

public class Book : BaseEntity
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required Category Category { get; set; }
}
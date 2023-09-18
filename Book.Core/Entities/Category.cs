using Book.Core.Base;

namespace Book.Core.Entities;

public class Category : BaseEntity
{
    public required string Name { get; set; }
}
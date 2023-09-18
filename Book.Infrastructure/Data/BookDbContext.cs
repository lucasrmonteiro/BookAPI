using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Book.Infrastructure.Data;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions options) : base(options)
    {
    }   
    
    public DbSet<Core.Entities.Book> Books { get; set; }
    public DbSet<Core.Entities.Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Book>(ConfigureBooks);
        modelBuilder.Entity<Core.Entities.Category>(ConfigureCategories);
    }

    private void ConfigureBooks(EntityTypeBuilder<Core.Entities.Book> builder)
    {
        builder.ToTable("Book");

        builder.HasKey(it => it.Id);
        
        builder.Property(it => it.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(it => it.Author)
            .IsRequired()
            .HasMaxLength(200);
    }
    
    private void ConfigureCategories(EntityTypeBuilder<Core.Entities.Category> builder)
    {
        builder.ToTable("Category");

        builder.HasKey(it => it.Id);
        
        builder.Property(it => it.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}
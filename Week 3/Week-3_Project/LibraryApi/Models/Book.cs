using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public Author? Author { get; set; }

    public ICollection<BookCategory> BookCategories { get; set; }
        = new List<BookCategory>();
}
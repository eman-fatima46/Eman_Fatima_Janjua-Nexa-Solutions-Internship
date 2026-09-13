using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    public string CategoryName { get; set; } = string.Empty;

    public ICollection<BookCategory> BookCategories { get; set; }
        = new List<BookCategory>();
}
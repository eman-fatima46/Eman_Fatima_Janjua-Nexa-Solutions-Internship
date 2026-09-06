using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models;

public class Author
{
    public int AuthorId { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; }
        = new List<Book>();
}
namespace LibraryApi.DTOs;

public class BookRequestDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
}
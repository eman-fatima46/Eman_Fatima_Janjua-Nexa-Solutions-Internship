using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models;

public class User
{
    public int UserId { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
}
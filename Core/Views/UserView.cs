using System.ComponentModel.DataAnnotations;

namespace Core.Views;

public class UserView
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [Range(1, 150)]
    public int Age { get; set; }
    [Required]
    public string Email { get; set; } = null!;
}

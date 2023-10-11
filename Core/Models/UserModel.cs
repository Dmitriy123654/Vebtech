using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

[Index("Email", IsUnique = true)]
public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    [Range(1, 150)]
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public List<RoleModel> Roles { get; set; } = null!;
}

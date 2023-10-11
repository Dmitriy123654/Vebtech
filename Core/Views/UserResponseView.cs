using Core.Models;

namespace Core.Views;

public class UserResponseView
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public ICollection<RoleModel> Roles { get; set; } = null!;
}
public static class UserResponseViewExtensionMethods
{
    public static UserResponseView ToView(this UserModel model)
    {
        return new UserResponseView
        {
            Id = model.Id,
            Name = model.Name,
            Age = model.Age,
            Email = model.Email,
            Roles = model.Roles
        };
    }
}


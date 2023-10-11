using Core.Enums;

namespace Core.Models;

public class RoleUserModel
{
    public RoleType RoleId { get; set; }
    public int UserId { get; set; }
}

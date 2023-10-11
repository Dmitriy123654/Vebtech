using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Models;

public class RoleModel
{
    public RoleType Id { get; set; }
    [JsonIgnore]
    public List<UserModel> Users { get; } = null!;
}

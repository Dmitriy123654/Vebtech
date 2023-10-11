using Core.Enums;

namespace Core.Options;

public class FilterOptions
{
    public List<RoleType>? ByRoles { get; set; }
    public string? ByEmail { get; set; }
    public string? ByName { get; set; }
    public int? ByAge { get; set; }
}

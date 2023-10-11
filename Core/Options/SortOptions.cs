using Core.Enums;

namespace Core.Options;

public class SortOptions
{
    public SortType? SortBy { get; set; } 
    public bool Asc { get; set; } = true;
}

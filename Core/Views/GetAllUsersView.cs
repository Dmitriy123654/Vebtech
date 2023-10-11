using Core.Options;

namespace Core.Views;

public class GetAllUsersView
{
    public FilterOptions? Filters { get; set; }
    public SortOptions? Sort { get; set; }
    public PaginationOptions Pagination { get; set; } = null!;
}

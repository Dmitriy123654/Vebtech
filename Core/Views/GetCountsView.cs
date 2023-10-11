using Core.Options;

namespace Core.Views;

public class GetCountsView
{
    public FilterOptions? Filters { get; set; }

    public PaginationCountOptions Pagination { get;set;} = null!;
}

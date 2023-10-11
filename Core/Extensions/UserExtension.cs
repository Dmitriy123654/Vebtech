using Core.Enums;
using Core.Models;
using Core.Options;

namespace Core.Extensions;

public static class UserExtension
{
    public static IQueryable<UserModel> Filter(this IQueryable<UserModel> query, FilterOptions filter)
    {
        if (filter == null) return query;

        if (filter.ByRoles != null && filter.ByRoles.Count != 0)
        {
            query = query.Where(user => user.Roles.Any(role => filter.ByRoles.Contains(role.Id)));
        }
        if (!string.IsNullOrEmpty(filter.ByEmail))
        {
            query = query.Where(user => user.Email.Contains(filter.ByEmail));
        }
        if (!string.IsNullOrEmpty(filter.ByName))
        {
            query = query.Where(user => user.Name.Contains(filter.ByName));
        }
        if (filter.ByAge != null)
        {
            query = query.Where(user => user.Age == filter.ByAge);
        }

        return query;
    }

    public static IQueryable<UserModel> Sort(this IQueryable<UserModel> query, SortOptions sort)
    {
        return sort?.SortBy switch
        {
            SortType.Name => sort.Asc ? query.OrderBy(user => user.Name) : query.OrderByDescending(user => user.Name),
            SortType.Age => sort.Asc ? query.OrderBy(user => user.Age) : query.OrderByDescending(user => user.Age),
            SortType.Email => sort.Asc ? query.OrderBy(user => user.Email) : query.OrderByDescending(user => user.Email),
            _ => query
        };
    }

}

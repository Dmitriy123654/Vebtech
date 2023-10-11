using BLL.Interfaces;
using Core.Views;
using DAL.Data;
using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Core.DTO;
using Core.Consts;
using Core.Extensions;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IValidationEmailService _validationEmailService;
    private readonly ApplicationDbContext _db;

    public UserService(ApplicationDbContext db, IValidationEmailService validationEmailService)
    {
        _validationEmailService = validationEmailService;
        _db = db;
    }

    public async Task<Result<List<UserResponseView>>> GetAllAsync(GetAllUsersView view)
    {
        var result = new Result<List<UserResponseView>>();

        var pagination = view.Pagination;
        var filters = view.Filters;
        var sort = view.Sort;
  
        var query = _db.Users
            .AsNoTracking()
            .Include(user => user.Roles)
            .AsQueryable()
            .Filter(filters)
            .Sort(sort);

        var totalCount = await query.CountAsync();

        var lastPage = totalCount < pagination.PageSize ? 1 : (int)Math.Ceiling((double)totalCount / pagination.PageSize);
        var currentPage = pagination.Page > lastPage ? lastPage : pagination.Page;
        var users = await query
            .Skip((currentPage - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(user => user.ToView())
            .ToListAsync();

        result.View = users;
        result.StatusCode = ResultType.Success;
        return result;
    }

    public async Task<Result<CountsResponseView>> GetCountAsync(GetCountsView view)
    {
        var result = new Result<CountsResponseView>();
        var countResponseView = new CountsResponseView();

        var pagination = view.Pagination;
        var filters = view.Filters;

        var query = _db.Users
            .AsNoTracking()
            .Include(user => user.Roles)
            .AsQueryable()
            .Filter(filters);

        var usersCount = await query.CountAsync();
        var pagesCount = (int)Math.Ceiling((double)usersCount / pagination.PageSize);

        countResponseView.UsersCount = usersCount;
        countResponseView.PagesCount = pagesCount;

        result.View = countResponseView;
        result.StatusCode = ResultType.Success;
        return result;
    }

    public async Task<Result<UserResponseView>> GetByIdAsync(int userId)
    {
        var result = new Result<UserResponseView>();

        var user = await _db.Users
            .AsNoTracking()
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user == null)
        {
            result.StatusCode = ResultType.NotFound;
            result.Error = ErrorConst.UserNotFound;
            return result;
        }

        result.View = user.ToView();
        result.StatusCode = ResultType.Success;
        return result;
    }

    public async Task<Result<UserResponseView>> SetRoles(int userId, SetRolesView view)
    {
        var result = new Result<UserResponseView>();

        var roles = view.Roles;

        var user = await _db.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user == null)
        {
            result.StatusCode = ResultType.NotFound;
            result.Error = ErrorConst.UserNotFound;
            return result;
        }

        // Deleting roles that are not contained in the database.
        var verifiedRoles = roles
            .Where(role => RoleType.IsDefined(typeof(RoleType), role))
            .Distinct();

        user.Roles = await _db.Roles
            .Where(role => verifiedRoles.Contains(role.Id))
            .ToListAsync();

        await _db.SaveChangesAsync();

        result.View = user.ToView();
        result.StatusCode = ResultType.Created;
        return result;
    }

    public async Task<Result<UserResponseView>> CreateAsync(UserView view)
    {
        var result = new Result<UserResponseView>();

        var newName = view.Name;
        var newEmail = view.Email;
        var newAge = view.Age;

        var IsEmailUnique = await _validationEmailService.IsEmailUniqueAsync(newEmail);
        if (!IsEmailUnique)
        {
            result.StatusCode = ResultType.Conflict;
            result.Error = ErrorConst.EmailExist;
            return result;
        }

        var role = await _db.Roles.FirstOrDefaultAsync(role => role.Id == RoleType.User);
        var newUser = new UserModel()
        {
            Name = newName,
            Age = newAge,
            Email = newEmail,
            Roles = new List<RoleModel>() { role! }
        };

        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();

        result.View = newUser.ToView();
        result.StatusCode = ResultType.Created;
        return result;
    }

    public async Task<Result<UserResponseView>> UpdateAsync(int userId, UserView view)
    {
        var result = new Result<UserResponseView>();

        var updatedName = view.Name;
        var updatedEmail = view.Email;
        var updatedAge = view.Age;

        var user = await _db.Users
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            result.StatusCode = ResultType.NotFound;
            result.Error = ErrorConst.UserNotFound;
            return result;
        }

        var isEmailUnique = true;
        var isEmailNew = user.Email != updatedEmail;
        if (isEmailNew)
        {
            isEmailUnique = await _validationEmailService.IsEmailUniqueAsync(updatedEmail);
        }
        if (!isEmailUnique)
        {
            result.StatusCode = ResultType.Conflict;
            result.Error = ErrorConst.EmailExist;
            return result;
        }

        user.Age = updatedAge;
        user.Email = updatedEmail;
        user.Name = updatedName;

        await _db.SaveChangesAsync();

        result.View = user.ToView();
        result.StatusCode = ResultType.Created;
        return result;
    }

    public async Task<Result<bool>> DeleteAsync(int userId)
    {
        var result = new Result<bool>();
        var response = await _db.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync();

        var isDeleted = Convert.ToBoolean(result);
        result.View = isDeleted;

        if (!isDeleted)
        {
            result.StatusCode = ResultType.NotFound;
            result.Error = ErrorConst.UserNotFound;
            return result;
        }

        result.StatusCode = ResultType.Success;
        return result;
    }
}

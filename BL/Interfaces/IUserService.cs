using Core.DTO;
using Core.Views;


namespace BLL.Interfaces;

public interface IUserService
{
    public Task<Result<List<UserResponseView>>> GetAllAsync(GetAllUsersView view);
    public Task<Result<CountsResponseView>> GetCountAsync(GetCountsView view);
    public Task<Result<UserResponseView>> GetByIdAsync(int userId);
    public Task<Result<UserResponseView>> SetRoles(int userId, SetRolesView view);
    public Task<Result<UserResponseView>> CreateAsync(UserView view);
    public Task<Result<UserResponseView>> UpdateAsync(int userId, UserView view);
    public Task<Result<bool>> DeleteAsync(int userId);
}

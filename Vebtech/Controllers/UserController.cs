using BLL.Interfaces;
using Core.Views;
using Microsoft.AspNetCore.Mvc;
using Core.Consts;
using Core.Enums;

namespace Vebtech.Controllers;

[ApiController]
[Route("api/users")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidationEmailService _validationEmailService;
    /// <summary>
    /// User Controller
    /// </summary>
    /// <param name="userService">this is a service that works with users</param>
    /// <param name="emailService">this service for validation email user's</param>
    public UserController(
        IUserService userService, 
        IValidationEmailService emailService)
    {
        _userService = userService;
        _validationEmailService = emailService;
    }

    /// <summary>
    /// Get a  collection of users.
    /// </summary>
    /// <remarks>
    /// This endpoint allows getting a collection of users based on filtering, sorting, and pagination.
    /// If there is no data on the specified page in the pagination, the last page with data will be returned.
    /// </remarks>
    /// <param name="view">The input data for getting data. Data consists of filtering(ByRoles: User = 1, Admin = 2, Support = 3, SuperAdmin = 4), sorting( Name = 1, Age = 2,Email = 3) (asc - true, desc - false), pagination.</param>
    /// <returns>
    /// - 200 OK if the operation is successful, with the collection of users in the response body.
    /// - 204 No Content if the operation is unsuccessful or has no data to return.
    /// </returns> 
    [HttpPost]
    [Route("all")]
    public async Task<IActionResult> GetAllAsync(GetAllUsersView view)
    {
        var result = await _userService.GetAllAsync(view);

        return result.StatusCode switch
        {
            ResultType.Success => Ok(result.View),
            _ => NoContent()
        };
    }

    /// <summary>
    ///  Retrieves the count of specific users based on the provided filters.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to retrieve the count of specific users based on the filters and page size specified in the input data.
    /// </remarks>
    /// <param name="view">The input data specifying the criteria for counting users (ByRoles: User = 1, Admin = 2, Support = 3, SuperAdmin = 4).</param>
    /// <returns>
    /// - 200 OK: If the count operation is successful, returns the count of items.
    /// - 204 No Content: If the count operation is unsuccessful or there are no items to count.
    /// </returns>
    [HttpPost]
    [Route("count")]
    public async Task<IActionResult> GetCountAsync(GetCountsView view)
    {
        if (view.Pagination.PageSize <= 0) return BadRequest(ErrorConst.InvalidData);
        var result = await _userService.GetCountAsync(view);

        return result.StatusCode switch
        {
            ResultType.Success => Ok(result.View),
            _ => NoContent()
        };
    }

    /// <summary>
    /// Retrieves user information by their unique identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to retrieve detailed information about a user by providing their unique identifier.
    /// </remarks>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>
    /// - 200 OK: If the user is found and the operation is successful, returns the user information.
    /// - 400 Bad Request: If an invalid userId (e.g., zero) is provided.
    /// - 404 Not Found: If the user is not found by the specified userId.
    /// - 204 No Content: If the operation is unsuccessful or there is no data to return.
    /// </returns>
    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetByIdAsync(int userId)
    {
        if (userId <= 0) return BadRequest(ErrorConst.InvalidData);

        var result = await _userService.GetByIdAsync(userId);

        return result.StatusCode switch
        {
            ResultType.Success => Ok(result.View),
            ResultType.NotFound => NotFound(result.Error),
            _ => NoContent()
        };

    }

    /// <summary>
    /// Sets roles for a user based on their unique identifier.
    /// </summary>
    /// <remarks>
    /// This method overwrites the roles for the user.
    /// This endpoint allows you to set roles for a user by providing their unique identifier and a list of roles to assign.
    /// </remarks>
    /// <param name="userId">The unique identifier of the user for whom roles are being set.</param>
    /// <param name="view">The input data specifying the roles to be assigned to the user.</param>
    /// <returns>
    /// - 201 Created: If the roles are successfully set, returns the updated user information.
    /// - 400 Bad Request: If the request is malformed or missing necessary data (e.g., userId, roles).
    /// - 404 Not Found: If the specified user is not found by the provided userId.
    /// - 204 No Content: If the operation is unsuccessful or there is no data to return.
    /// </returns>
    [HttpPut("{userId}/roles")]
    public async Task<IActionResult> SetRoles(int userId, SetRolesView view)
    {
        var isBadRequest = userId <= 0 || view.Roles == null || view.Roles.Count == 0;
        if (isBadRequest) return BadRequest(ErrorConst.InvalidData);

        var result = await _userService.SetRoles(userId, view);

        return result.StatusCode switch
        {
            ResultType.Created => Ok(result.View),
            ResultType.NotFound => NotFound(result.Error),
            _ => NoContent()
        };
    }

    /// <summary>
    /// Create a new user based on the provided user information.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to create a new user by providing user information. The request body should contain valid user data.
    /// The default role for a user is User.
    /// </remarks>
    /// <param name="user">The user information to create a new user.</param>
    /// <returns>
    /// - 201 Created: If the user is successfully created, returns the created user information.
    /// - 400 Bad Request: If the request is malformed, missing necessary data, or the provided user data is invalid.
    /// - 409 Conflict: If there is a conflict, such as an attempt to create a user with a duplicate identifier.
    /// - 204 No Content: If the operation is unsuccessful or there is no data to return.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(UserView user)
    {
        if (user == null) return BadRequest(ErrorConst.InvalidData);
        if(!_validationEmailService.IsValidEmail(user!.Email)) return BadRequest(ErrorConst.InvalidEmail);
        var result = await _userService.CreateAsync(user);

        return result.StatusCode switch
        {
            ResultType.Created => Ok(result.View),
            ResultType.Conflict => Conflict(result.Error),
            _ => NoContent()
        };
    }

    /// <summary>
    /// Update an existing user based on the provided user information and user identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to update an existing user by providing the user's unique identifier and updated user information. The request body should contain valid user data.
    /// </remarks>
    /// <param name="userId">The unique identifier of the user to be updated.</param>
    /// <param name="view">The input data specifying for update user's fields.</param>
    /// <returns>
    /// - 201 Created: If the user is successfully updated, returns the updated user information.
    /// - 400 Bad Request: If the request is malformed, missing necessary data, or the provided user data is invalid.
    /// - 404 Not Found: If the specified user is not found by the provided userId.
    /// - 409 Conflict: If there is a conflict, such as an attempt to update a user with a duplicate identifier.
    /// - 204 No Content: If the operation is unsuccessful or there is no data to return.
    /// </returns>
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAsync(int userId, UserView view)
    {
        var isBadRequest = userId <= 0 || view == null;
        if (isBadRequest) return BadRequest();
        if (!_validationEmailService.IsValidEmail(view!.Email)) return BadRequest(ErrorConst.InvalidEmail);

        var result = await _userService.UpdateAsync(userId, view);

        return result.StatusCode switch
        {
            ResultType.Created => Ok(result.View),
            ResultType.NotFound => NotFound(result.Error),
            ResultType.Conflict => Conflict(result.Error),
            _ => NoContent()
        };
    }

    /// <summary>
    /// Delete a user based on their unique identifier.
    /// </summary>
    /// <remarks>
    /// This endpoint allows you to delete a user by providing their unique identifier.
    /// </remarks>
    /// <param name="userId">The unique identifier of the user to be deleted.</param>
    /// <returns>
    /// - 200 OK: If the user is successfully deleted, returns a response indicating success.
    /// - 400 Bad Request: If an invalid userId (e.g., zero) is provided.
    /// - 404 Not Found: If the specified user is not found by the provided userId.
    /// - 204 No Content: If the operation is unsuccessful or there is no data to return.
    /// </returns>
    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> DeleteAsync(int userId)
    {
        if (userId <= 0) return BadRequest(ErrorConst.InvalidData);

        var result = await _userService.DeleteAsync(userId);

        return result.StatusCode switch
        {
            ResultType.Success => Ok(result.View),
            ResultType.NotFound => NotFound(result.Error),
            _ => NoContent()
        };
    }
}

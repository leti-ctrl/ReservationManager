using Microsoft.AspNetCore.Mvc.Filters;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Authorization;

public class RoleAuthorizationFilter: IAsyncActionFilter
{
    private readonly IUserService _userService;
    private readonly string[] _validRoles;

    public RoleAuthorizationFilter(IUserService userService, string[] validRoles)
    {
        _userService = userService;
        _validRoles = validRoles;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claim = context.HttpContext.Request.Query["email"];
        if (claim.Count == 0) 
            throw new BadHttpRequestException("Missing email claim.");

        var userRoles = await _userService.GetUserByEmail(claim.ToString());
        if(userRoles == null) 
            throw new NonExistentUserException($"User {claim.ToString()} does not exist.");

        if (userRoles.Roles.Any(role => _validRoles.Contains(role.Code)))
            await next();
        else
            throw new UnauthorizedAccessException("User does not have valid roles for this action.");
    }
}
using Microsoft.AspNetCore.Mvc.Filters;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Authorization;

public class RoleAuthorizationFilterFactory : Attribute, IFilterFactory
{
    private readonly string[] _validRoles;

    public bool IsReusable => false;

    public RoleAuthorizationFilterFactory(string[] validRoles)
    {
        _validRoles = validRoles;
    }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new RoleAuthorizationFilter(serviceProvider.GetRequiredService<IUserService>(), _validRoles);
    }
}
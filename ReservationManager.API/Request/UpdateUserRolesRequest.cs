using System.ComponentModel.DataAnnotations;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Request;

public class UpdateUserRolesRequest
{
    [MinLength(1, ErrorMessage = "User roles must not be empty")]
    public RoleDto[] Roles { get; set; } = null!;
}
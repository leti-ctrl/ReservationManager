using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Authorization;
using ReservationManager.API.Controllers.Base;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.HumanResources })]

    public class RoleController : SessionController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();

            if (result == null || !result.Any())
                return NoContent();
            return Ok(result);
        }

        
    }
}

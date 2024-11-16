using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllUserTypes()
        {
            var result = await _roleService.GetAllUserTypes();

            if (result == null || !result.Any())
                return NoContent();
            return Ok(result);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDto>> CreateUserType(string code, string name)
        {
            var result = await _roleService.CreateUserType(code, name);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDto>> UpdateUserType(int id, string code, string name)
        {
            var result = await _roleService.UpdateUserType(id, code, name);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserType(int id)
        {
            await _roleService.DeleteUserType(id);
            return Accepted();

        }
    }
}

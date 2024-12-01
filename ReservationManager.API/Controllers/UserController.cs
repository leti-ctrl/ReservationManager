
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Authorization;
using ReservationManager.API.Controllers.Base;
using ReservationManager.API.Request;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin , FixedUserRole.HumanResources })]

    public class UserController : SessionController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserInfo(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (!users.Any())
                return NoContent();

            return Ok(users);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> CreateUser(UserUpsertRequest request)
        {
            var created = await _userService.CreateUser(request.Adapt<UpsertUserDto>());

            return Ok(created);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpsertRequest request)
        {
            var updated = await _userService.UpdateUser(id, request.Adapt<UpsertUserDto>());
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
        
        [HttpPatch("{id}/role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUserRole(int id, [FromBody] UpdateUserRolesRequest request)
        {
            var roles = request.Roles.Adapt<Role[]>();
            var user =await _userService.UpdateUserRoles(id, roles);
            return Ok(user);
        }

        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Accepted();
        }
    }
}

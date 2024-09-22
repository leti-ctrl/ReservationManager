
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Request.User;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (!users.Any())
                return NoContent();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUser(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserUpsertRequest user)
        {
            var created = await _userService.CreateUser(user.Adapt<UpsertUserDto>());

            return Ok(created);
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpsertRequest user)
        {
            var updated = await _userService.UpdateUser(id, user.Adapt<UpsertUserDto>());
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Accepted();
        }
    }
}

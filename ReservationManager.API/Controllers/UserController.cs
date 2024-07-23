
using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UpsertUserDto user)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpsertUserDto user)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeService _userTypeService;

        public UserTypeController(IUserTypeService userTypeService)
        {
            _userTypeService = userTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTypeDto>>> GetAllUserTypes()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<UserTypeDto>> CreateUserType(UpsertUserDto user)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<UserTypeDto>> UpdateUserType(int id, UpsertUserDto user)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUserType(int id)
        {
            throw new NotImplementedException();
        }
    }
}

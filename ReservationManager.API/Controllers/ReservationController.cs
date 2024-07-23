using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations(int userId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(UpsertReservationDto reservation)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<ReservationDto>> UpdateReservation(int id, UpsertReservationDto reservation)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            throw new NotImplementedException();
        }
    }
}

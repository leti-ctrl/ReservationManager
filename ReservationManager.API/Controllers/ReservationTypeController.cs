using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationTypeController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationTypeDto>>> GetAllReservationTypes()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<ReservationTypeDto>> CreateReservationType(string code, TimeOnly start, TimeOnly end)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<ReservationTypeDto>> UpdateReservationType(int id, ReservationTypeDto reservation)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteReservationType(int id)
        {
            throw new NotImplementedException();
        }
    }
}

using Mapster;
using Microsoft.AspNetCore.Mvc;
using ReservationManager.API.Authorization;
using ReservationManager.API.Controllers.Base;
using ReservationManager.API.Request;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorizationFilterFactory(new[] { FixedUserRole.Admin })]
    public class ReservationTypeController : SessionController
    {
        private readonly IReservationTypeService _reservationTypeService;

        public ReservationTypeController(IReservationTypeService reservationTypeService)
        {
            _reservationTypeService = reservationTypeService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ReservationTypeDto>>> GetAllReservationTypes()
        {
            var reservationTypes = await _reservationTypeService.GetAllReservationType();

            if (!reservationTypes.Any())
                return NoContent();
            return Ok(reservationTypes);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationTypeDto>> CreateReservationType(ReservationTypeUpsertRequest request)
        {
            var created = await _reservationTypeService.CreateReservationType(request.Adapt<UpsertReservationTypeDto>());
            return Ok(created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReservationTypeDto>> UpdateReservationType(int id, ReservationTypeUpsertRequest request)
        {
            var updated = await _reservationTypeService.UpdateReservationType(id, request.Adapt<UpsertReservationTypeDto>());
            if(updated == null)
                return NotFound();
            
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteReservationType(int id)
        {
            await _reservationTypeService.DeleteReservationType(id);
            return Accepted();
        }
    }
}

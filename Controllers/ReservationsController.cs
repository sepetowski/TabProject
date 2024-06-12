using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Loan;
using TabProjectServer.Models.DTO.Reservations;
using TabProjectServer.Services;

namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _reservationsService;

        public ReservationsController(IReservationsService reservationsService)
        {
            _reservationsService = reservationsService;
        }


        [HttpGet()]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _reservationsService.GetAllReservationsAsync();

            return Ok(res);
        }

        [HttpGet("GetUserReservations/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserReservations([FromRoute] Guid id)
        {
            var res = await _reservationsService.GetUserReservationsAsync(id);

            if (res == null) return BadRequest("User not found");
            return Ok(res);
        }

        [HttpPost("CreateReservation")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationReqDTO req)
        {
            try
            {

                var res = await _reservationsService.CreateReservationAsync(req);

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("CancelReservation")]
        [Authorize]
        public async Task<IActionResult> CancelReservation([FromBody] CancelReservationReqDTO req)
        {
                var res = await _reservationsService.CancelReservationAsync(req);

                if (res == null)
                    return BadRequest("Reservation not found");

                return Ok(res);
            
        }

    }
}

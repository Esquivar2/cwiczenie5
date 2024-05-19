using Microsoft.AspNetCore.Mvc;
using Zadanie7.Models.DTOs;
using Zadanie7.Repositories;

namespace Zadanie7.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : Controller
    {
        private readonly ITripsRepository _tripsRepository;
        public TripsController(ITripsRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var result = await _tripsRepository.GetTripsAsync();
            return Ok(result);  
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripDTO client)
        {
          
            try
            {
                var result = await _tripsRepository.AddClientToTripAsync(idTrip, client);

                if (result == -1)
                {
                    return NotFound(new { message = "Trip not found." });
                }

                if (result == 0)
                {
                    return Conflict(new { message = "Client is already booked for this trip." });
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Error", error = e.Message });
            }



        }
    }
}
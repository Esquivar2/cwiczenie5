using System;
using Microsoft.AspNetCore.Mvc;
using Zadanie7.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zadanie7.Controllers
{

    [Route("api/clients")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly ITripsRepository _tripsRepository;
        public ClientsController(ITripsRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }

        [HttpDelete("{idClient:int}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            try
            {
                var result = await _tripsRepository.DeleteClientAsync(idClient);

                if (result == 0)
                {
                    return NotFound(new { message = "Client not found." });
                }

                if (result == -1)
                {
                    return BadRequest(new { message = "Client has associated trips and cannot be deleted." });
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
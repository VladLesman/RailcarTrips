using Microsoft.AspNetCore.Mvc;

using RailcarTrips.Application.Contracts;

namespace RailcarTrips.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripQueryService _service; 

        public TripsController(ITripQueryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _service.GetTripsAsync();
            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripDetail(int id)
        {
            if (id == 0) return NotFound();
            var trip = await _service.GetTripWithEventsAsync(id);
            if (trip == null) return NotFound();

            return Ok(trip);
        }
    }
}

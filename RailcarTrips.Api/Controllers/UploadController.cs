using Microsoft.AspNetCore.Mvc;

using RailcarTrips.Application.Contracts;

namespace RailcarTrips.Api.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly ITripImportService _service;
        private readonly ICitiesImportService _sitiesImportService;

        public UploadController(ITripImportService service, ICitiesImportService sitiesImportService)
        {
            _service = service;
            _sitiesImportService = sitiesImportService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            await _service.ImportAsync(file.OpenReadStream());
            return Ok();
        }

        [HttpPost("cities")]
        public async Task<IActionResult> UploadCities(IFormFile file)
        {
            await _sitiesImportService.ImportAsync(file.OpenReadStream());
            return Ok();
        }
    }

}

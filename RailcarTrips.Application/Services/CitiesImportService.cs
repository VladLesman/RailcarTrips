using RailcarTrips.Application.Contracts;
using RailcarTrips.Core.Contracts;
using RailcarTrips.Core.Entities;
using RailcarTrips.Infrastructure.Contracts;
using RailcarTrips.Infrastructure.Entities;
using RailcarTrips.Infrastructure.Parsers;
namespace RailcarTrips.Application.Services
{
    public class CitiesImportService : ICitiesImportService
    {
        private readonly ICitiesRepository _sitiesRepository;

        public CitiesImportService(
            ICitiesRepository sitiesRepository)
        {
            _sitiesRepository = sitiesRepository;
        }

        public async Task ImportAsync(Stream csvStream)
        {
            var records = CsvParser.ParseCities(csvStream);

            var domainSities = records.Select(r =>
            {
                return new CityEntity
                {
                    Id = r.Id,
                    Name = r.Name,  
                    TimeZoneId = r.TimeZoneId
                };
            }).ToList();

            await _sitiesRepository.UploadCitiesAsync(domainSities);
        }
    }
}

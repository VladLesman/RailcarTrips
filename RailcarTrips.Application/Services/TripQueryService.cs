using RailcarTrips.Application.Contracts;
using RailcarTrips.Infrastructure.Contracts;
using RailcarTrips.Infrastructure.Entities;
using RailcarTrips.Infrastructure.Repositories;
using RailcarTrips.Shared.Dtos;

namespace RailcarTrips.Application.Services
{
    namespace RailcarTrips.Application.Services
    {
        public class TripQueryService : ITripQueryService
        {
            private readonly ITripRepository _repo;
            private readonly ICitiesRepository _citiesRepository;

            public TripQueryService(ITripRepository repo, ICitiesRepository citiesRepository)
            {
                _repo = repo;
                _citiesRepository = citiesRepository;
            }

            public async Task<TripDto> GetTripWithEventsAsync(int id)
            {
                var trip = await _repo.GetTripWithEventsAsync(id);

                var tripDto = new TripDto
                {
                    Id = trip.Id,
                    EquipmentId = trip.EquipmentId,
                    OriginCityName = await _citiesRepository.GetCityNameAsync(trip.OriginCityId),
                    DestinationCityName = await _citiesRepository.GetCityNameAsync(trip.DestinationCityId),
                    StartUtc = trip.StartUtc,
                    EndUtc = trip.EndUtc,
                    TotalHours = trip.TotalHours,
                    Events = new List<TripEventDto>()
                };

                foreach (var e in trip.Events.OrderBy(e => e.UtcTime))
                {
                    var cityName = await _citiesRepository.GetCityNameAsync(e.CityId);

                    tripDto.Events.Add(new TripEventDto
                    {
                        EventCode = e.EventCode,
                        CityName = cityName,
                        LocalTime = e.LocalTime,
                        UtcTime = e.UtcTime
                    });
                }

                return tripDto;
            }

            public async Task<IEnumerable<TripDto>> GetTripsAsync()
            {
                var trips = await _repo.GetTripsAsync();

                var result = new List<TripDto>();

                foreach (var t in trips)
                {
                    var dto = new TripDto
                    {
                        Id = t.Id,
                        EquipmentId = t.EquipmentId,
                        OriginCityName = await _citiesRepository.GetCityNameAsync(t.OriginCityId),
                        DestinationCityName = await _citiesRepository.GetCityNameAsync(t.DestinationCityId),
                        StartUtc = t.StartUtc,
                        EndUtc = t.EndUtc,
                        TotalHours = t.TotalHours
                    };

                    result.Add(dto);
                }

                return result;
            }

            public async Task<Dictionary<int, string>> GetCitiesAsync()
            {
                var cities = await _citiesRepository.GetCitiesAsync();
                // return dictionary of CityId -> CityName
                return cities.ToDictionary(c => c.Key, c => c.Value.Name);
            }
        }
    }
}

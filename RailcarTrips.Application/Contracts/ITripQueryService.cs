using RailcarTrips.Shared.Dtos;

namespace RailcarTrips.Application.Contracts
{
    public interface ITripQueryService
    {
        Task<TripDto> GetTripWithEventsAsync(int id);
        Task<IEnumerable<TripDto>> GetTripsAsync();
        Task<Dictionary<int, string>> GetCitiesAsync();
    }
}

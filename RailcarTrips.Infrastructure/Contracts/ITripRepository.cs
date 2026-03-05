using RailcarTrips.Core.Entities;
using RailcarTrips.Infrastructure.Entities;

namespace RailcarTrips.Infrastructure.Contracts
{
    public interface ITripRepository
    {
        Task<IEnumerable<TripEntity>> GetTripsAsync();
        Task<TripEntity> GetTripWithEventsAsync(int id);
        Task SaveTripsAsync(IEnumerable<Trip> trips);
    }
}

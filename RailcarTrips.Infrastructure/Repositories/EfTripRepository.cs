using Microsoft.EntityFrameworkCore;

using RailcarTrips.Core.Entities;
using RailcarTrips.Infrastructure.Contracts;
using RailcarTrips.Infrastructure.Entities;

namespace RailcarTrips.Infrastructure.Repositories
{
    public class EfTripRepository : ITripRepository
    {
        private readonly AppDbContext _db;

        public EfTripRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TripEntity>> GetTripsAsync()
        {
            return await _db.Trips
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TripEntity> GetTripWithEventsAsync(int id)
        {
            return await _db.Trips
                .Include(t => t.Events.OrderBy(e => e.UtcTime))
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task SaveTripsAsync(IEnumerable<Trip> trips)
        {
            foreach (var trip in trips)
            {
                var entity = new TripEntity
                {
                    EquipmentId = trip.EquipmentId,
                    OriginCityId = trip.OriginCityId,
                    DestinationCityId = trip.DestinationCityId,
                    StartUtc = trip.StartUtc,
                    EndUtc = trip.EndUtc,
                    TotalHours = trip.TotalHours,
                    Events = trip.Events.Select(e => new EquipmentEventEntity
                    {
                        EquipmentId = e.EquipmentId,
                        EventCode = e.EventCode,
                        CityId = e.CityId,
                        LocalTime = e.LocalTime,
                        UtcTime = e.UtcTime
                    }).ToList()
                };
                _db.Trips.Add(entity);
            }

            await _db.SaveChangesAsync();
        }
    }
}

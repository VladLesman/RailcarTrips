using RailcarTrips.Core.Entities;

namespace RailcarTrips.Core.Contracts
{
    public interface ITripProcessor
    {
        IEnumerable<Trip> BuildTrips(
            IEnumerable<EquipmentEvent> events,
            Func<int, string> resolveCityTimeZone);
    }
}

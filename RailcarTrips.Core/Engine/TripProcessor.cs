using RailcarTrips.Core.Contracts;
using RailcarTrips.Core.Entities;

namespace RailcarTrips.Core.Engine
{
    public class TripProcessor : ITripProcessor
    {
        public IEnumerable<Trip> BuildTrips(IEnumerable<EquipmentEvent> events, Func<int, string> resolveCityTimeZone)
        {
            var trips = new List<Trip>();
            var grouped = events.GroupBy(e => e.EquipmentId);

            foreach (var group in grouped)
            {
                var ordered = group.OrderBy(e => e.UtcTime).ToList();
                Trip currentTrip = null;

                foreach (var e in ordered)
                {
                    // Start new trip on W
                    if (e.EventCode == "W")
                    {
                        // Close previous trip if still open
                        if (currentTrip != null)
                        {
                            currentTrip.DestinationCityId = currentTrip.Events.Last().CityId;
                            currentTrip.EndUtc = currentTrip.Events.Last().UtcTime;
                            trips.Add(currentTrip);
                        }

                        currentTrip = new Trip
                        {
                            EquipmentId = e.EquipmentId,
                            OriginCityId = e.CityId,
                            StartUtc = e.UtcTime
                        };

                        currentTrip.Events.Add(e);
                        continue;
                    }

                    // If trip is open, add events
                    if (currentTrip != null)
                    {
                        currentTrip.Events.Add(e);

                        // Close on Z
                        if (e.EventCode == "Z")
                        {
                            currentTrip.DestinationCityId = e.CityId;
                            currentTrip.EndUtc = e.UtcTime;
                            trips.Add(currentTrip);
                            currentTrip = null;
                        }
                    }
                }

                // If trip ended without Z → close with last event
                if (currentTrip != null)
                {
                    currentTrip.DestinationCityId = currentTrip.Events.Last().CityId;
                    currentTrip.EndUtc = currentTrip.Events.Last().UtcTime;
                    trips.Add(currentTrip);
                }
            }

            return trips;
        }
    }
}

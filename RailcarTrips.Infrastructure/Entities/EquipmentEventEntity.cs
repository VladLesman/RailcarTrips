namespace RailcarTrips.Infrastructure.Entities
{
    public class EquipmentEventEntity
    {
        public int Id { get; set; }
        public string EquipmentId { get; set; }
        public string EventCode { get; set; }

        public int CityId { get; set; }

        public DateTime LocalTime { get; set; }
        public DateTime UtcTime { get; set; }

        public int? TripId { get; set; }
        public TripEntity Trip { get; set; }
    }
}

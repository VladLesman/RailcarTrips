namespace RailcarTrips.Infrastructure.Entities
{
    public class TripEntity
    {
        public int Id { get; set; }
        public string EquipmentId { get; set; }

        public int OriginCityId { get; set; }
        public int DestinationCityId { get; set; }

        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }

        public double TotalHours { get; set; }

        public List<EquipmentEventEntity> Events { get; set; } = new();
    }
}

namespace RailcarTrips.Core.Entities
{
    public class Trip
    {
        public string EquipmentId { get; init; }
        public int OriginCityId { get; set; }
        public int DestinationCityId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }

        public double TotalHours =>
            (EndUtc - StartUtc).TotalHours;

        public List<EquipmentEvent> Events { get; } = new();
    }

}

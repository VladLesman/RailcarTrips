namespace RailcarTrips.Shared.Dtos
{
    public class TripDto
    {
        public int Id { get; set; }
        public string EquipmentId { get; set; }
        public string OriginCityName { get; set; }
        public string DestinationCityName { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public double TotalHours { get; set; }
        public List<TripEventDto> Events { get; set; } = new();
    }
}

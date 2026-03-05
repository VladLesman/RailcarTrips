namespace RailcarTrips.Core.Entities
{
    public class EquipmentEvent
    {
        public string EquipmentId { get; init; }
        public string EventCode { get; init; } 
        public int CityId { get; init; }
        public DateTime LocalTime { get; init; }
        public DateTime UtcTime { get; set; }
    }

}

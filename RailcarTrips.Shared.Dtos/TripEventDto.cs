namespace RailcarTrips.Shared.Dtos
{
    public class TripEventDto
    {
        public string EventCode { get; set; }
        public string CityName { get; set; }
        public DateTime LocalTime { get; set; }
        public DateTime UtcTime { get; set; }
    }
}

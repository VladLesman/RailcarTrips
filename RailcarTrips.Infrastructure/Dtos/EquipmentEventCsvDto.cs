using CsvHelper.Configuration.Attributes;

namespace RailcarTrips.Infrastructure.Dtos
{
    public class EquipmentEventCsvDto
    {
        [Name("Equipment Id")] 
        public string EquipmentId { get; set; }
        [Name("Event Code")] 
        public string EventCode { get; set; }
        [Name("Event Time")] 
        public string EventTime { get; set; }
        [Name("City Id")] 
        public int CityId { get; set; }
    }
}

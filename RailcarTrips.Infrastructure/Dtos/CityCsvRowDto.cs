using CsvHelper.Configuration.Attributes;

namespace RailcarTrips.Infrastructure.Dtos
{
    public class CityCsvRowDto
    {
        [Name("City Id")]
        public int Id { get; set; }

        [Name("City Name")]
        public string Name { get; set; }

        [Name("Time Zone")]
        public string TimeZoneId { get; set; }
    }

}

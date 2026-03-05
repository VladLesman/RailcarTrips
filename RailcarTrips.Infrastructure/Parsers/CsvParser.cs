using CsvHelper;

using RailcarTrips.Infrastructure.Dtos;

using System.Globalization;

namespace RailcarTrips.Infrastructure.Parsers
{
    public static class CsvParser
    {
        public static IEnumerable<EquipmentEventCsvDto> ParseEvents(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return [.. csv.GetRecords<EquipmentEventCsvDto>()];
        }

        public static IEnumerable<CityCsvRowDto> ParseCities(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return [.. csv.GetRecords<CityCsvRowDto>()];
        }
    }
}

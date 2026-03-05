using RailcarTrips.Application.Contracts;
using RailcarTrips.Core.Contracts;
using RailcarTrips.Core.Entities;
using RailcarTrips.Infrastructure.Contracts;
using RailcarTrips.Infrastructure.Parsers;

namespace RailcarTrips.Application.Services
{
    public class TripImportService : ITripImportService
    {
        private readonly ITripProcessor _processor;
        private readonly ITimeZoneConverter _tz;
        private readonly ITripRepository _repository;
        private readonly ICitiesRepository _sitiesRepository;

        public TripImportService(
            ITripProcessor processor,
            ITimeZoneConverter tz,
            ITripRepository repository,
            ICitiesRepository sitiesRepository)
        {
            _processor = processor;
            _tz = tz;
            _repository = repository;
            _sitiesRepository = sitiesRepository;
        }

        public async Task ImportAsync(Stream csvStream)
        {
            var records = CsvParser.ParseEvents(csvStream);

            var cities = await _sitiesRepository.GetCitiesAsync();

            var domainEvents = new List<EquipmentEvent>();

            foreach (var r in records)
            {
                try
                {
                    if (!DateTime.TryParse(r.EventTime, out var local))
                    {
                        // TODO: log invalid date
                        continue;
                    }

                    var utc = _tz.ConvertToUtc(local, cities[r.CityId].TimeZoneId);

                    domainEvents.Add(new EquipmentEvent
                    {
                        EquipmentId = r.EquipmentId,
                        EventCode = r.EventCode,
                        CityId = r.CityId,
                        LocalTime = local,
                        UtcTime = utc
                    });
                }
                catch (Exception exc)
                {
                    // TODO: log
                }
            }

            var trips = _processor.BuildTrips(domainEvents, cityId => cities[cityId].TimeZoneId);

            await _repository.SaveTripsAsync(trips);
        }
    }
}

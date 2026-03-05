using FluentAssertions;

using RailcarTrips.Core.Engine;
using RailcarTrips.Core.Entities;

namespace RailcarTrips.Core.Tests
{
    public class TripProcessorTests
    {
        [Fact]
        public void Should_Build_Trip_From_W_To_Z()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent
                {
                    EquipmentId = "A",
                    EventCode = "W",
                    UtcTime = new DateTime(2024,1,1,0,0,0,DateTimeKind.Utc),
                    CityId = 1
                },
                new EquipmentEvent
                {
                    EquipmentId = "A",
                    EventCode = "Z",
                    UtcTime = new DateTime(2024,1,2,0,0,0,DateTimeKind.Utc),
                    CityId = 2
                }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(1);
            trips[0].TotalHours.Should().Be(24);
        }

        [Fact]
        public void Should_Build_Multiple_Trips_For_Same_Equipment()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(2), CityId = 2 },

                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(3), CityId = 3 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(4), CityId = 4 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(2);
            trips[0].OriginCityId.Should().Be(1);
            trips[0].DestinationCityId.Should().Be(2);
            trips[1].OriginCityId.Should().Be(3);
            trips[1].DestinationCityId.Should().Be(4);
        }

        [Fact]
        public void Should_Ignore_Events_Before_First_W()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "A", UtcTime = Utc(1), CityId = 10 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "D", UtcTime = Utc(2), CityId = 11 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(3), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(4), CityId = 2 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(1);
            trips[0].OriginCityId.Should().Be(1);
        }

        [Fact]
        public void Should_Ignore_Events_After_Last_Z()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(2), CityId = 2 },

                new EquipmentEvent { EquipmentId = "A", EventCode = "A", UtcTime = Utc(3), CityId = 99 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "D", UtcTime = Utc(4), CityId = 100 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(1);
            trips[0].DestinationCityId.Should().Be(2);
        }

        [Fact]
        public void Should_Sort_Events_By_UtcTime()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(3), CityId = 2 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "A", UtcTime = Utc(2), CityId = 10 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(1);
            trips[0].OriginCityId.Should().Be(1);
            trips[0].DestinationCityId.Should().Be(2);
        }

        [Fact]
        public void Should_Not_Mix_Events_From_Different_Equipment()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "B", EventCode = "W", UtcTime = Utc(2), CityId = 5 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(3), CityId = 2 },
                new EquipmentEvent { EquipmentId = "B", EventCode = "Z", UtcTime = Utc(4), CityId = 6 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(2);
            trips.Count(t => t.EquipmentId == "A").Should().Be(1);
            trips.Count(t => t.EquipmentId == "B").Should().Be(1);
        }

        [Fact]
        public void Should_Keep_Events_In_Order_Inside_Trip()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "A", UtcTime = Utc(2), CityId = 10 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(3), CityId = 2 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips[0].Events.Select(e => e.EventCode).Should().ContainInOrder("W", "A", "Z");
        }
        [Fact]
        public void Should_Close_Trip_Without_Z_Using_Last_Event()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "A", UtcTime = Utc(2), CityId = 10 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(1);
            trips[0].EndUtc.Should().Be(Utc(2));
        }

        [Fact]
        public void Should_Not_Create_Trip_If_No_W()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "A", UtcTime = Utc(1), CityId = 10 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(2), CityId = 2 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().BeEmpty();
        }

        [Fact]
        public void Should_Handle_Multiple_Z_In_Sequence()
        {
            var processor = new TripProcessor();

            var events = new[]
            {
                new EquipmentEvent { EquipmentId = "A", EventCode = "W", UtcTime = Utc(1), CityId = 1 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(2), CityId = 2 },
                new EquipmentEvent { EquipmentId = "A", EventCode = "Z", UtcTime = Utc(3), CityId = 3 }
            };

            var trips = processor.BuildTrips(events, _ => "").ToList();

            trips.Should().HaveCount(1);
            trips[0].DestinationCityId.Should().Be(2); // first Z closes trip
        }


        private static DateTime Utc(int hour) =>
            new DateTime(2024, 1, 1, hour, 0, 0, DateTimeKind.Utc);

    }
}
using RailcarTrips.Core.Contracts;

namespace RailcarTrips.Infrastructure.Converters
{
    public class TimeZoneConverter : ITimeZoneConverter
    {
        public DateTime ConvertToUtc(DateTime localTime, string timeZoneId)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            if (tz.IsInvalidTime(localTime))
            {
                localTime = localTime.AddHours(1);
            }

            return TimeZoneInfo.ConvertTimeToUtc(localTime, tz);
        }
    }
}

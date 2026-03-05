namespace RailcarTrips.Core.Contracts
{
    public interface ITimeZoneConverter
    {
        DateTime ConvertToUtc(DateTime localTime, string timeZoneId);
    }
}

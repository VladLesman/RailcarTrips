using RailcarTrips.Infrastructure.Entities;

namespace RailcarTrips.Infrastructure.Contracts
{
    public interface ICitiesRepository
    {
        Task<Dictionary<int, CityEntity>> GetCitiesAsync();

        Task UploadCitiesAsync(IEnumerable<CityEntity> cities);

        Task<string> GetCityNameAsync(int cityId);
    }
}

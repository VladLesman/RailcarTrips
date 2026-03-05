using Microsoft.EntityFrameworkCore;

using RailcarTrips.Infrastructure.Contracts;
using RailcarTrips.Infrastructure.Entities;

namespace RailcarTrips.Infrastructure.Repositories
{
    public class EfCitiesRepository : ICitiesRepository
    {
        private readonly AppDbContext _db;

        private Dictionary<int, CityEntity>? _cache;


        public EfCitiesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Dictionary<int, CityEntity>> GetCitiesAsync()
        {
            if (_cache != null) return _cache;

            _cache = await _db.Cities.ToDictionaryAsync(c => c.Id);

            return _cache;
        }

        public async Task<string> GetCityNameAsync(int cityId)
        {
            var cities = await GetCitiesAsync();
            if (cities.TryGetValue(cityId, out var city))
            {
                return city.Name;
            }
            return "Unknown";
        }

        public async Task UploadCitiesAsync(IEnumerable<CityEntity> cities)
        {
            _cache = null; // Invalidate cache
            foreach (var row in cities) 
            { 
                if (!_db.Cities.Any(c => c.Id == row.Id))
                { 
                    _db.Cities.Add(row);
                }
            }
            await _db.SaveChangesAsync();
        }
    }
}

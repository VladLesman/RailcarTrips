using Microsoft.AspNetCore.Components.Forms;

using RailcarTrips.Shared.Dtos;

using System.Net.Http.Json;

namespace RailcarTrips.Services
{
    public class TripApiClient
    {
        private readonly HttpClient _http;

        public TripApiClient(HttpClient http) => _http = http;

        public async Task<List<TripDto>> GetTripsAsync()
            => await _http.GetFromJsonAsync<List<TripDto>>("api/trips") ?? [];

        public async Task<TripDto?> GetTripDetailAsync(int id)
            => await _http.GetFromJsonAsync<TripDto?>($"api/trips/{id}");

        public async Task UploadCsvAsync(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file.OpenReadStream()), "file", file.Name);
            var response = await _http.PostAsync("api/upload", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UploadCitiesCsvAsync(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file.OpenReadStream()), "file", file.Name);
            var response = await _http.PostAsync("api/upload/cities", content);
            response.EnsureSuccessStatusCode();
        }
    }
}

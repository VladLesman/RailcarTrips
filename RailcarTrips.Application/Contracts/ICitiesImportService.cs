namespace RailcarTrips.Application.Contracts
{
    public interface ICitiesImportService
    {
        Task ImportAsync(Stream csvStream);
    }
}

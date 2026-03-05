namespace RailcarTrips.Application.Contracts
{
    public interface ITripImportService
    {
        Task ImportAsync(Stream csvStream);
    }
}

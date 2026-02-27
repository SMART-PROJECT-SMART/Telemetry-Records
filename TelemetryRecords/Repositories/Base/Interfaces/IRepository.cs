namespace TelemetryRecords.Repositories.Base.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetLatestAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    }
}

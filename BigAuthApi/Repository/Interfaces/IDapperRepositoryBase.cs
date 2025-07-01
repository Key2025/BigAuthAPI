namespace BigAuthApi.Repository.Interfaces
{
    public interface IDapperRepositoryBase
    {
        Task<IEnumerable<T>> QueryDataAsync<T>(string query, object? param = null, int commandTimeout = 180);

        Task<int> ExecuteDataAsync(string query, object? param = null, int commandTimeout = 180);
    }
}
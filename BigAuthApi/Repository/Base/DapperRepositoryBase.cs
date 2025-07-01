using BigAuthApi.Exceptions;
using BigAuthApi.Repository.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace BigAuthApi.Repository.Base
{
    public class DapperRepositoryBase : IDapperRepositoryBase
    {
        private readonly string _connectionString;
        private readonly ILogger<DapperRepositoryBase> _logger;

        public DapperRepositoryBase(IConfiguration configuration, ILogger<DapperRepositoryBase> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> ExecuteDataAsync(string query, object? param = null, int commandTimeout = 180)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            try
            {
                _logger.LogInformation("Executing ExecuteAsync: {Query} with parameters: {Parameters}", query, JsonConvert.SerializeObject(param));

                return await connection.ExecuteAsync(
                    query,
                    param,
                    commandTimeout: commandTimeout,
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error occurred while executing: {Query}", query);
                throw new BaseException(
                    $"DbBase::ExecuteAsync SP failed:: [{query}] parameter : {JsonConvert.SerializeObject(param)} :{sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing: {Query}", query);
                throw new BaseException(
                    $"DbBase::ExecuteAsync SP failed:: [{query}] parameter : {JsonConvert.SerializeObject(param)} :{ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<T>> QueryDataAsync<T>(string query, object? param = null, int commandTimeout = 180)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            try
            {
                _logger.LogInformation("Executing QueryAsync: {Query} with parameters: {Parameters}", query, JsonConvert.SerializeObject(param));

                return await connection.QueryAsync<T>(
                    query,
                    param,
                    commandTimeout: commandTimeout,
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error occurred while executing query: {Query}", query);
                throw new BaseException(
                    $"DbBase::QueryAsync SP failed:: [{query}] parameter : {JsonConvert.SerializeObject(param)} :{sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing query: {Query}", query);
                throw new BaseException(
                    $"DbBase::QueryAsync SP failed:: [{query}] parameter : {JsonConvert.SerializeObject(param)} :{ex.Message}", ex);
            }
        }
    }
}
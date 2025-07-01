using BigAuthApi.Exceptions;
using BigAuthApi.Repository.Interfaces;
using Newtonsoft.Json;

namespace BigAuthApi.Repository.Base
{
    public class BaseRepository
    {
        private readonly IDapperRepositoryBase _dapperBase;
        private readonly ILogger<BaseRepository> _logger;

        public BaseRepository(IDapperRepositoryBase dapperBase, ILogger<BaseRepository> logger)
        {
            _dapperBase = dapperBase ?? throw new ArgumentNullException(nameof(dapperBase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> ExecuteDataAsync(string storedProcedureId, object? param = null)
        {
            try
            {
                return await _dapperBase.ExecuteDataAsync(storedProcedureId, param, 180);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "StoredProcedure: '{StoredProcedureId}', Param: {Param}, ErrorCode: '{ErrorCode}'",
                    storedProcedureId, JsonConvert.SerializeObject(param), e.HResult);

                throw new BaseException($"Error executing stored procedure '{storedProcedureId}'", e);
            }
        }

        public async Task<IEnumerable<T>> GetDataAsync<T>(string storedProcedureId, object? param = null)
        {
            try
            {
                return await _dapperBase.QueryDataAsync<T>(storedProcedureId, param, 180);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "StoredProcedure: '{StoredProcedureId}', Param: {Param}, ErrorCode: '{ErrorCode}'",
                    storedProcedureId, JsonConvert.SerializeObject(param), e.HResult);

                throw new BaseException($"Error executing stored procedure '{storedProcedureId}'", e);
            }
        }
    }
}
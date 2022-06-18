using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SV.HRM.DataProcess.Infrastructure.Interfaces
{
    public interface IDapperReposity
    {
        IDbConnection GetDbConnection();

        //Sync
        T ExecuteScalar<T>(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        int Execute(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        T QuerySingleOrDefault<T>(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        List<object> QueryMultiple(string sql, object parameters, IDbTransaction trans = null, CommandType? commandType = null, params Func<Dapper.SqlMapper.GridReader, object>[] readerFuncs);
        IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        //Async
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction trans = null, CommandType? commandType = null);
        int ExecuteScalarTransactionSPMulti(List<string> listSpName, List<object> listparam);
        int ExecuteScalarTransactionQueryMulti(List<string> listQuery, List<object> listparam);
        Task<int> ExecuteScalarTransactionSPMultiAsync(List<string> listSpName, List<object> listparam);
        Task<int> ExecuteScalarTransactionQueryMultiAsync(List<string> listQuery, List<object> listparam);
        DataTable ExecuteReport(string sql, object param , IDbTransaction trans = null, CommandType? commandType = null);
    }
}

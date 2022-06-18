using SV.HRM.DataProcess.Infrastructure.Interfaces;
using System;
using System.Data;
namespace SV.HRM.DataProcess.Infrastructure.Impl
{
    public class DapperUnitOfWork : IDapperUnitOfWork
    {
        public readonly IDbConnection _connection;
        private bool _disposed;

        public DapperUnitOfWork(IDbConnection dbConnection)
        {
            _connection = dbConnection;
            _connection.Open();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {

                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }

            _disposed = true;
        }

        public IDapperReposity GetRepository()
        {
            return new DapperReposity(_connection);
        }
    }
}

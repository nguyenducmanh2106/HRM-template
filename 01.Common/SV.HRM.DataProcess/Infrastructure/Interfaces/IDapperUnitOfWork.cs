using System;
namespace SV.HRM.DataProcess.Infrastructure.Interfaces
{
    public interface IDapperUnitOfWork : IDisposable
    {
        IDapperReposity GetRepository();
    }
}

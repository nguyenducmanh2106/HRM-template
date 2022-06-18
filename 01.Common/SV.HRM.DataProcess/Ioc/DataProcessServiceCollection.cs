using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Impl;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace SV.HRM.DataProcess.Ioc
{
    public static class DataProcessServiceCollection
    {
        public static void AddDataProcessServices(this IServiceCollection services)
        {
            var connectionsDic = AppSettings.Instance.Get<Dictionary<string, string>>("Databases:MSSQL:ConnectionStrings");
            var savisConnection = connectionsDic["SavisCoreFWEntities"];
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(savisConnection));
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();
        }
    }
}

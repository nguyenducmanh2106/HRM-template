using SV.HRM.Logging.NLogCustom;
using SV.HRM.Logging.StaticConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;

namespace SV.HRM.Logging.Extensions
{
    public static class LoggingBuilderExten
    {
        public static void UseSerilog(this ILoggingBuilder builder, IConfiguration configuration)
        {
            //builder.AddSerilog();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration, "Logging:Providers:Serilog")
            //    .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.Logger(configuration =>
            //    {
            //        configuration.WriteTo.File("Logs/error.log");
            //        configuration.Filter.ByIncludingOnly(ev => ev.)
            //    })
            //    .CreateLogger();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder">Using ILogging builder</param>
        /// <param name="configFile">nlog.config file</param>
        /// <param name="kafkaTaget">Ip and port of Kafka cluster</param>
        /// <param name="applicationStore">Application name and Application Id</param>
        public static void UseNLog(string configFile, string kafkaTaget, LogSourceTypeEnums applicationStore)
        {
            NLogTargetCustom.RegisterTarget();
            StaticConfiguration.KafkaServer = kafkaTaget;
            StaticConfiguration.ApplicationStore = new Dictionary<int, string> { { (int)applicationStore, EnumConvert.GetEnumDescription(applicationStore) } };
            NLogBuilder.ConfigureNLog(configFile);
        }
    }
}
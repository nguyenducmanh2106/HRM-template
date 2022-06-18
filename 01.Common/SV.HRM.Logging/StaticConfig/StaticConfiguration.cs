using System;
using System.Collections.Generic;
using System.Text;

namespace SV.HRM.Logging.StaticConfig
{
    public static class StaticConfiguration
    {
        public static string KafkaServer { get; set; } = "2.2.2.83:57909";
        public static Dictionary<int, string> ApplicationStore = new Dictionary<int, string>();
    }
}

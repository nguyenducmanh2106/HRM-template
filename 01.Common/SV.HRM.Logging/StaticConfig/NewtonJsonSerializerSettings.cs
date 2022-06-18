using System.Text.Json;

namespace SV.HRM.Logging.StaticConfig
{
    public static class JsonSerializerSettings
    {
        public static readonly JsonSerializerOptions CAMEL = new JsonSerializerOptions
        {

            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
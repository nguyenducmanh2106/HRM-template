using Microsoft.AspNetCore.Http;
using SV.HRM.Core.Utils;

namespace SV.HRM.Caching.Common
{
    public class CacheHelpers
    {
        public static bool IsRequestClearCache(HttpContext context = null, string refreshKey = null)
        {
            if (context == null) return false;

            if (context.Request != null && (context.Request.Headers != null && !string.IsNullOrWhiteSpace(context.Request.Headers[Constant.RequestHeader.USER_AGENT].ToString())))
            {
                if (string.IsNullOrEmpty(refreshKey))
                {
                    return context.Request.Headers[Constant.RequestHeader.USER_AGENT].ToString().Contains("refreshcache");
                }
                else
                {
                    return context.Request.Headers[Constant.RequestHeader.USER_AGENT].ToString().Contains(refreshKey);
                }
            }
            return false;
        }
    }
}

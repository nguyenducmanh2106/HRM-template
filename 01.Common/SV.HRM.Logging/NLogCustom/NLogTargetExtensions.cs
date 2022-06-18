using SV.HRM.Logging.KafkaTarget;
using NLog.LayoutRenderers;
using NLog.Targets;
using System;

namespace SV.HRM.Logging.NLogCustom
{
    public static class NLogTargetCustom
    {
        public static void RegisterTarget()
        {
            Target.Register<KafkaAppender>("KafkaAppender");
            //Layout.Register<HelloWorldLayoutRenderer>("test");
            //LayoutRenderer.Register("user_name", typeof(IdentityLayoutRenderer));
            LayoutRenderer.Register("ip_client", typeof(IpClientLayoutRenderer));
            LayoutRenderer.Register("app_name", typeof(ApplicationLayoutRenderer));
            LayoutRenderer.Register("app_id", typeof(ApplicationIdLayoutRenderer));
            LayoutRenderer.Register("time_span", _ => DateTime.Now.ToUniversalTime()
                .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));
            LayoutRenderer.Register("messages_info", typeof(MessagesInfoLayoutRenderer));
            LayoutRenderer.Register("params_info", typeof(ParamsInfoLayoutRenderer));
        }
    }
}

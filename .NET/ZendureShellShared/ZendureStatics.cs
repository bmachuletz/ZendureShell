using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public static class ZendureStatics
    {

        public static string APP_USERNAME = string.Empty;
        public static string APP_PASSWORD = string.Empty;

        private static string APP_WEBAPI_HOSTNAME = "app.zendure.tech/eu";
        private static readonly string APP_HOSTNAME_US = "app.zendure.tech";
        //  private static readonly string APP_VERSION = "v2";

        private static readonly string APP_AUTH_PATH = "/auth/app/token";
        private static readonly string APP_DETAIL_PATH = "/device/solarFlow/detail";
        private static readonly string APP_DEVICELIST_PATH = "/productModule/device/queryDeviceListByConsumerId";
        private static readonly string APP_DEVELOPER_PATH = "developer/api/apply";

     //   public static string        APP_MQTT_SERVER = @"mqtteu.zen-iot.com";
        public static readonly int  APP_MQTT_PORT = 1883;
     //   public static string        APP_MQTT_USER;
     //   public static string        APP_MQTT_PASSWORD = @"H6s$j9CtNa0N";

        public static readonly string APP_AUTH_URL =        $"https://{APP_HOSTNAME_US}/v2/{APP_AUTH_PATH}";
        public static readonly string APP_DEVICELIST_URL =  $"https://{APP_WEBAPI_HOSTNAME}/{APP_DEVICELIST_PATH}";
        public static readonly string APP_DETAILS_URL =     $"https://{APP_WEBAPI_HOSTNAME}/{APP_DETAIL_PATH}";
        public static readonly string APP_DEVELOPER_URL =   $"https://{APP_WEBAPI_HOSTNAME}/{APP_DEVELOPER_PATH}";

        public static Dictionary<string, string> AUTH_HEADER = new Dictionary<string, string>()
        {
            { "Accept-Language", "de-DE" },
            { "appVersion", "4.3.1" },
            { "User-Agent", "Zendure/4.3.1 (iPhone; iOS 14.4.2; Scale/3.00)" },
            { "Accept", "*/*" },
            { "Authorization", "Basic Q29uc3VtZXJBcHA6NX4qUmRuTnJATWg0WjEyMw==" },
            { "Blade-Auth", "bearer (null)" }
        };

        public static Dictionary<string, string> DEVICE_DETAIL_BODY = new Dictionary<string, string>()
        {
            {"deviceId", "" },
        };

        public static Dictionary<string, string> AUTH_BODY = new Dictionary<string, string>()
        {
            { "password", APP_PASSWORD },
            { "account", APP_USERNAME },
            { "appId", "121c83f761305d6cf7e" },
            { "appType", "iOS" },
            { "grantType", "password" },
            { "tenantId", "" }
        };
    }
}

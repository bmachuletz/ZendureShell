﻿using System;
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

        private static readonly string APP_HOSTNAME = "app.zendure.tech";
        private static readonly string APP_VERSION = "v2";

        private static readonly string APP_AUTH_PATH = "/auth/app/token";
        private static readonly string APP_DETAIL_PATH = "/device/solarFlow/detail";
        private static readonly string APP_DEVICELIST_PATH = "/productModule/device/queryDeviceListByConsumerId";

        public static readonly string APP_AUTH_URL = $"https://{APP_HOSTNAME}/{APP_VERSION}/{APP_AUTH_PATH}";
        public static readonly string APP_DEVICELIST_URL = $"https://{APP_HOSTNAME}/{APP_VERSION}/{APP_DEVICELIST_PATH}";
        public static readonly string APP_DETAILS_URL = $"https://{APP_HOSTNAME}/{APP_VERSION}/{APP_DETAIL_PATH}";

        public static Dictionary<string, string> AUTH_HEADER = new Dictionary<string, string>()
        {
         //   { "Content-Type", "application/json" },
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
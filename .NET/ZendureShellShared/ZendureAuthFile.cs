using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class ZendureAuthFile
    {
        [JsonProperty("AccountName")]
        public string AccountName;
        [JsonProperty("Password")]
        public string Password;
        [JsonProperty("Blade-Auth")]
        public string BladeAuth;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public string userType { get; set; }
        public string accessToken { get; set; }
        public string userId { get; set; }
        public string tenantId { get; set; }
        public string oauthId { get; set; }
        public string avatar { get; set; }
        public string authority { get; set; }
        public string userName { get; set; }
        public string account { get; set; }
        public string countryCode { get; set; }
    }

    public class ZendureAuthResponse : IZendureResponse
    {
        public int code { get; set; }
        public bool success { get; set; }
        public Data data { get; set; }
        public string msg { get; set; }


        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public string DataToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.data);
        }
    }


}

namespace ZendureShellShared
{
    public class DeveloperResponseData
    {
        public string appKey { get; set; }
        public string secret { get; set; }
        public string mqttUrl { get; set; }
        public string port { get; set; }
    }

 
    public class ZendureDeveloperApiResponse : IZendureResponse
    {
        public int code { get; set; }
        public bool success { get; set; }
        public DeveloperResponseData data { get; set; }
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

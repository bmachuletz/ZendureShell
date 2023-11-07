using Newtonsoft.Json;


namespace ZendureShellShared
{
    public class MqttProperties
    {
        [JsonProperty("outputLimit", NullValueHandling = NullValueHandling.Ignore)]
        public int outputLimit { get; set; }
    }


    public class ZendureMqttProperties
    {
        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public MqttProperties properties { get; set; }

        public ZendureMqttProperties(MqttProperties _prop)
        {
            properties = _prop;
        }

        public ZendureMqttProperties()
        {
        }
    }
}

using MQTTnet.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace ZendureShell
{


    public class WriteTemplate
    {
        public WriteTemplate() 
        { 
            properties = new Dictionary<string, object>();
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        public Int32 messageId = 23;
        public Dictionary<string, dynamic> properties;
        public string deviceId = "q22vn4A9";
        public string timestamp;
        

    }


    public class MqttMessage
    {
        [JsonIgnore]
        public string Topic { get; set; }
        [JsonIgnore]
        public string Data { get; set; }

        [JsonPropertyName("properties")]
        public WriteTemplate template;
    }

    public class Mqtt_Template_SetProperty : MqttMessage
    {
        public Mqtt_Template_SetProperty(string Property, dynamic Value) 
        {
            WriteTemplate template = new WriteTemplate();
            template.properties.Add(Property, Value);

            /*
             * {"properties": { "outputLimit": 300 } }
             */

            Topic = $"iot/[AppKey]/[DeviceId]/properties/write";

            Data = Newtonsoft.Json.JsonConvert.SerializeObject(template, Newtonsoft.Json.Formatting.None);
        }
    }
}

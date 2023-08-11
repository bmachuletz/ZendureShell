using System.Collections.Generic;

namespace ZendureShellShared
{
    public class ZendureMqttClientSubscriber : IZendureMqttClientSubscriber
    {
        private List<string>? subscribedTopics = new List<string>();
        public List<string> SubscribedTopics { get => subscribedTopics; set => subscribedTopics = value; }

        ZendureMqttClientSubscriber(ref IZendureMqttClient zendureMqttClient)
        {
            if(zendureMqttClient.ZendureMqttClientVariant == ZendureMqttClientVariant.DEVELOPER_MQTT)
            {


            }
            else if(zendureMqttClient.ZendureMqttClientVariant == ZendureMqttClientVariant.ZENDURE_MQTT)
            {
                
            }
        }



        public void Subscribe()
        {
           
        }
    }
}
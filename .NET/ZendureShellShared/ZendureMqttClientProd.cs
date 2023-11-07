using MQTTnet.Client;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public  class ZendureMqttClientProd : ZendureMqttClient     {
        public ZendureMqttClientProd() : base(ZendureMqttClientVariant.ZENDURE_MQTT)
        {
            clientOptions = new MqttClientOptionsBuilder()
            .WithClientId(_authToken)
            .WithTcpServer(ZendureStatics.APP_MQTT_SERVER, ZendureStatics.APP_MQTT_PORT)
            .WithCredentials(ZendureStatics.APP_MQTT_USER, ZendureStatics.APP_MQTT_PASSWORD)
            .WithCleanSession()
            .Build();

            LoadAsync().Wait();
        }

        private async Task LoadAsync()
        {
            base.Connect();
          
            //  base.Subscribe();
        }
    }
}

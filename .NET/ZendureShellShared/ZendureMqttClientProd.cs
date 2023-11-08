using MQTTnet.Client;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public  class ZendureMqttClientProd : ZendureMqttClient     
    {
        public ZendureMqttClientProd() : base(ZendureMqttClientVariant.ZENDURE_MQTT)
        {
            var data = FileHandler.LoadConfigFromAppData();

            clientOptions = new MqttClientOptionsBuilder()
            .WithClientId(data.BearerToken)
            .WithTcpServer(data.IotUrl, ZendureStatics.APP_MQTT_PORT)
            .WithCredentials(data.IotUsername, data.IotPassword)
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

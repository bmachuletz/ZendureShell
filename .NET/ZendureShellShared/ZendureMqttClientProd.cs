using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public  class ZendureMqttClientProd : ZendureMqttClient
    {
        public ZendureMqttClientProd() : base(ZendureMqttClientVariant.ZENDURE_MQTT)
        {
            

            clientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
                .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId(zendureCredentials.BearerToken)
                .WithTcpServer(ZendureStatics.APP_MQTT_SERVER, ZendureStatics.APP_MQTT_PORT)
                .WithCredentials(ZendureStatics.APP_MQTT_USER, ZendureStatics.APP_MQTT_PASSWORD)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                .WithTimeout(TimeSpan.FromSeconds(30))
                .WithCleanSession()).Build();

            LoadAsync().Wait();
        }

        private async Task LoadAsync()
        {
            base.Connect();

            base.Subscribe()
        }
    }
}

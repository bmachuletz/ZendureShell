

using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class ZendureMqttClientDev : ZendureMqttClient
    {

        public ZendureMqttClientDev(ref IZendureMqttClient zendureMqttClient) : base(ZendureMqttClientVariant.DEVELOPER_MQTT)
        {
            /*
            clientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
                .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId("111")
                .WithTcpServer(zendureMqttClient., Convert.ToInt32(x.data.port))
                .WithCredentials(x.data.appKey, x.data.secret)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                .WithTimeout(TimeSpan.FromSeconds(30))
                .WithCleanSession()).Build();
            */
            
            base.Connect();
        }


        public async Task LoadAsync()
        {
            ZendureApiWrapper zendureApiWrapperDev = new ZendureApiWrapper(_serialNumber, _username);
            var x = await zendureApiWrapperDev.GetDeveloperToken() as ZendureDeveloperApiResponse;

        }
    }
}

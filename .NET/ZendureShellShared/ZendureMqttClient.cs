using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace ZendureShellShared
{
    public class ZendureMqttClient 
    {
        public event EventHandler<ZendureMqttMessageArrivedEventArgs> MessageArrived;


        private Func<MqttApplicationMessageReceivedEventArgs, Task> receivedMessageCallback;
        private IManagedMqttClient managedMqttClient;

        public ZendureMqttClient(string Hostname, Int32 Port, string ClientId)
        {
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId(ClientId)
                .WithCredentials("xxxx", "xxxx")
                .WithTcpServer(Hostname, Port)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .WithCleanSession(true)
                .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)).Build();


            managedMqttClient = new MqttFactory().CreateManagedMqttClient();

            managedMqttClient.StartAsync(options);

            if (!managedMqttClient.IsConnected) while (managedMqttClient.IsConnected == false) { Task.Delay(250); };

            managedMqttClient.ApplicationMessageReceivedAsync += async (e) =>
            {
                var message = new ZendureMqttMessageArrivedEventArgs();
                message.Topic = e.ApplicationMessage.Topic;
                message.Message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                message.Hostname = Hostname;
                MessageArrived?.Invoke(this, message);
            };

         //   managedMqttClient.SubscribeAsync($"{appKey}/#");
         //   managedMqttClient.SubscribeAsync("/server/app/xxxxx/#");
        }


        public void Subscribe(string Topic)
        {
            managedMqttClient.SubscribeAsync(Topic);
        }

    }
}

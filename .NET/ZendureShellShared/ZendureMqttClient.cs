using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class ZendureMqttClient : IZendureMqttClient
    {

        private AutoResetEvent resetEvent = new AutoResetEvent(false);
        public ZendureCredentials zendureCredentials;

        public event EventHandler<ZendureMqttMessageArrivedEventArgs> MessageArrived;
        private Func<MqttApplicationMessageReceivedEventArgs, Task> receivedMessageCallback;
        
        private MQTTnet.Client.MqttClient managedMqttClient;

        public MqttClientOptions clientOptions;

        public IZendureMqttClientSubscriber subscriber;

        private ZendureMqttClientVariant zendureMqttClientVariant;
        public ZendureMqttClientVariant ZendureMqttClientVariant { get => zendureMqttClientVariant; }

        //private string appKey;
        //private List<string> deviceKey;
        //public string AppKey { get => appKey; }
        //public List<string> DeviceKeys { get => deviceKey; }

        /*
        public string _password = string.Empty;
        */

        public ZendureMqttClient(ZendureMqttClientVariant zendureMqttClientVariant)
        {
            zendureCredentials = new ZendureCredentials();
            zendureCredentials.Fill().Wait();

            this.zendureMqttClientVariant = zendureMqttClientVariant;
            managedMqttClient = new MqttFactory().CreateMqttClient() as MQTTnet.Client.MqttClient;

        }

        public async Task InitAsync()
        {
            /*
            ZendureApiWrapper zendureApiWrapperRest = new ZendureApiWrapper(_username, _password, _authToken);
            var y = await zendureApiWrapperRest.GetDeviceList() as ZendureDeviceListResponse;
            y.data.ForEach(device =>
            {
                DeviceKeys.Add(device.deviceKey);
            });

            appKey = x.data.appKey;
            */
        }

        public void Connect()
        {
           string clienId = zendureCredentials.BearerToken.Replace("bearer ", string.Empty);        
           managedMqttClient.ConnectAsync(clientOptions);
           if (!managedMqttClient.IsConnected) while (managedMqttClient.IsConnected == false) { Task.Delay(250); };
        }

        public void Disconnect()
        {
            managedMqttClient.DisconnectAsync();
        }

        public void Subscribe(string topic)
        {   
            if (managedMqttClient.IsConnected == false)
            {
                Connect();
            }
            else
            {
                managedMqttClient.SubscribeAsync(topic);
            }
        }

        
        public void Publish(string topic, string payload)
        {
            if (managedMqttClient.IsConnected == false)
            {
                Connect();
            }
            else
            {
                MqttApplicationMessage message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                managedMqttClient.PublishAsync(message);
            }
        }
        
    }
}

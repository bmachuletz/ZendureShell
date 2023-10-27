using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class ZendureMqttClient : IZendureMqttClient
    {
        public ZendureCredentials zendureCredentials;

        public event EventHandler<ZendureMqttMessageArrivedEventArgs> MessageArrived;
        private Func<MqttApplicationMessageReceivedEventArgs, Task> receivedMessageCallback;
        
        private IManagedMqttClient managedMqttClient;

        public ManagedMqttClientOptions clientOptions;

        public IZendureMqttClientSubscriber subscriber;

        private ZendureMqttClientVariant zendureMqttClientVariant;
        public ZendureMqttClientVariant ZendureMqttClientVariant { get => zendureMqttClientVariant; }

        private string appKey;
        private List<string> deviceKey;
        public string AppKey { get => appKey; }
        public List<string> DeviceKeys { get => deviceKey; }

        public string _serialNumber = string.Empty;
        public string _username = string.Empty;
        public string _password = string.Empty;
        public string _authToken = string.Empty;

        public ZendureMqttClient(ZendureMqttClientVariant zendureMqttClientVariant)
        {
            zendureCredentials = new ZendureCredentials();
            zendureCredentials.Fill().Wait();

            _serialNumber = zendureCredentials.SerialNumber;
            _username = zendureCredentials.AccountName;
            _password = zendureCredentials.Password;
            _authToken = zendureCredentials.BearerToken;

            this.zendureMqttClientVariant = zendureMqttClientVariant;
            managedMqttClient = new MqttFactory().CreateManagedMqttClient();

            
  //          subscriber = new ZendureMqttClientSubscriber(managedMqttClient);
  //          subscriber.MessageArrived += Subscriber_MessageArrived;
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
            clientOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
            .WithClientOptions(new MqttClientOptionsBuilder()
            .WithClientId(zendureCredentials.BearerToken.Replace("bearer ", string.Empty))
            .WithTcpServer(ZendureStatics.APP_MQTT_SERVER, ZendureStatics.APP_MQTT_PORT)
            .WithCredentials(ZendureStatics.APP_MQTT_USER, ZendureStatics.APP_MQTT_PASSWORD)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
            .WithTimeout(TimeSpan.FromSeconds(30))
            .WithCleanSession()).Build();

            managedMqttClient.SubscribeAsync("xxxxxxxxxxxxxxxxxxxxx");

        //    managedMqttClient.StartAsync(clientOptions).Wait();
    

           // managedMqttClient.InternalClient.ConnectAsync().Wait();

        //    if (!managedMqttClient.IsConnected) while (managedMqttClient.IsConnected == false) { Task.Delay(250); };
        }

        public void Disconnect()
        {
            managedMqttClient.StopAsync();
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

                managedMqttClient.InternalClient.PublishAsync(message);
            }
        }
    }
}

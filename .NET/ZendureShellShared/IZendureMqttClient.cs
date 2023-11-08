using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public interface IZendureMqttClient
    {
        public event EventHandler<ZendureMqttMessageArrivedEventArgs> MessageArrived;
        public ZendureMqttClientVariant ZendureMqttClientVariant { get; }

     //   public string AppKey { get; }
     //   public List<string> DeviceKeys { get; }

        public void Connect();
        public void Disconnect();
        public void Publish(string topic, string payload);

    }
}

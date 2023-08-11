using System.Collections.Generic;

namespace ZendureShellShared
{
    public interface IZendureMqttClientSubscriber
    {
        public List<string> SubscribedTopics { get; set; }
        public void Subscribe();
    }
}

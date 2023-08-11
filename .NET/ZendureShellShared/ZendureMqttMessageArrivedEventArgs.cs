using System;

namespace ZendureShellShared
{
    public class ZendureMqttMessageArrivedEventArgs : EventArgs
    {
        public string Topic { get; set; }
        public string Message { get; set; }
        public string Hostname { get; set; }
    }
}
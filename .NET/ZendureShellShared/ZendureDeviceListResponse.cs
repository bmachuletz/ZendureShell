using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class Device
    {
        public int id { get; set; }
        public string deviceKey { get; set; }
        public string snNumber { get; set; }
        public string name { get; set; }
        public int productId { get; set; }
        public string productKey { get; set; }
        public string onlineFlag { get; set; }
        public string productName { get; set; }
        public bool wifiStatus { get; set; }
        public bool blueState { get; set; }
        public bool fourGStatus { get; set; }
        public string isShareFlag { get; set; }
        public bool input { get; set; }
        public bool output { get; set; }
        public int electricity { get; set; }
        public bool upsMode { get; set; }
        public string upgradeStatusDes { get; set; }
        public int productType { get; set; }
        public UpgradeStatus upgradeStatus { get; set; }
        public int bindId { get; set; }
        public int bindStatus { get; set; }
        public string batteryCode { get; set; }
        public List<object> packList { get; set; }
        public int inputPower { get; set; }
        public int outputPower { get; set; }
        public int slowChargePower { get; set; }
        public int temperature { get; set; }
        public int temperatureUnit { get; set; }
        public int remainOutTime { get; set; }
        public int bindType { get; set; }
        public int seriesMode { get; set; }
        public int parallelMode { get; set; }
        public int networkType { get; set; }
        public string standard { get; set; }
        public bool isSwitch { get; set; }
    }

    public class ZendureDeviceListResponse : IZendureResonse
    {
        public int code { get; set; }
        public bool success { get; set; }
        public List<Device> data { get; set; }
        public string msg { get; set; }
    }

    public class UpgradeStatus
    {
    }

}

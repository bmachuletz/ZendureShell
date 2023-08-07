using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class DeviceDetailData
    {
        public bool isShare { get; set; }
        public string productKey { get; set; }
        public int productModelId { get; set; }
        public string productName { get; set; }
        public string deviceName { get; set; }
        public int deviceId { get; set; }
        public string deviceKey { get; set; }
        public string snNumber { get; set; }
        public string onlineFlag { get; set; }
        public int id { get; set; }
        public int consumerId { get; set; }
        public bool masterSwitch { get; set; }
        public int hubState { get; set; }
        public bool wifiState { get; set; }
        public string wifiName { get; set; }
        public string mac { get; set; }
        public string ip { get; set; }
        public bool buzzerSwitch { get; set; }
        public int solarInputPower { get; set; }
        public int packInputPower { get; set; }
        public int outputPackPower { get; set; }
        public int outputHomePower { get; set; }
        public int outputLimit { get; set; }
        public int inputLimit { get; set; }
        public int remainOutTime { get; set; }
        public int remainInputTime { get; set; }
        public int socSet { get; set; }
        public int minSoc { get; set; }
        public int packState { get; set; }
        public int packNum { get; set; }
        public int electricLevel { get; set; }
        public int isDeleted { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public int inverseMaxPower { get; set; }
        public int brandId { get; set; }
        public int cycle { get; set; }
        public int blueOta { get; set; }
        public int solarInputPowerCycle { get; set; }
        public int packInputPowerCycle { get; set; }
        public int outputPackPowerCycle { get; set; }
        public int outputHomePowerCycle { get; set; }
        public List<PackDataList> packDataList { get; set; }
        public double todayEnergy { get; set; }
        public int temperatureUnit { get; set; }
        public bool faultFlag { get; set; }
        public bool bateUser { get; set; }
        public int deviceId2 { get; set; }
    }

    public class PackDataList
    {
        public int createUser { get; set; }
        public DateTime createTime { get; set; }
        public int updateUser { get; set; }
        public DateTime updateTime { get; set; }
        public int isDeleted { get; set; }
        public int id { get; set; }
        public int deviceId { get; set; }
        public int consumerId { get; set; }
        public int socLevel { get; set; }
        public int power { get; set; }
        public int state { get; set; }
        public double maxTemp { get; set; }
        public int totalVol { get; set; }
        public int maxVol { get; set; }
        public int minVol { get; set; }
        public string batteryCode { get; set; }
        public int softVersion { get; set; }
        public string version { get; set; }
        public string sn { get; set; }
    }

    public class ZendureDeviceDetailsResponse : IZendureResponse
    {
        public int code { get; set; }
        public bool success { get; set; }
        public DeviceDetailData data { get; set; }
        public string msg { get; set; }

        public string ToJson()
        {       
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public string DataToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.data);
        }
    }
}

﻿using System;
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

        }
    }
}
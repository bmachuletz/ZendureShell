# Autor: Benjamin Machuletz
# Datum: 2023-08-02
# Version: 0.1
# Beschreibung: Get the Developer Key for the Zendure MQTT-API 
# (https://github.com/Zendure/developer-device-data-report)
#
# License: MIT License
# License URI: https://opensource.org/licenses/MIT

$url = "https://app.zendure.tech/eu"

$ZendureDeveloperUrl = "$url/developer/api/apply";

# Seriennummer eurer SolarFlow Box
$serialNumber = "xxxxxxxxxxxxx";

# Benutzername eures Zendure Accounts in der App
$appUsername = "xxxxxxxxx@xxxx.de";

$ZendureDeveloperBody = @{ 
                            "snNumber" = "$serialNumber";
                            "account"  = "$appUsername" 
                        } | ConvertTo-Json;

                
$response = Invoke-WebRequest -Method Post -UseBasicParsing -Uri $ZendureDeveloperUrl -Body $ZendureDeveloperBody -ContentType "application/json";

$developerData = $response | ConvertFrom-Json | Select-Object -ExpandProperty data;

$developerData

<#
    $developerData.appKey;
    $developerData.secret;
    $developerData.mqttZendureDeveloperUrl$ZendureDeveloperUrl;
    $developerData.mqttPort;
#>

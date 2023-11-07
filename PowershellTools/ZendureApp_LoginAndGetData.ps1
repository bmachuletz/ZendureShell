# Autor: Benjamin Machuletz
# Datum: 2023-08-02
# Version: 0.1
# Beschreibung: Abfragen der Daten der SolarFlow Box wie die Zendure App.
#               Die Steuerung der SolarFlow Box ist hiermit möglich.
# (https://github.com/Zendure/developer-device-data-report)
#
# License: MIT License
# License URI: https://opensource.org/licenses/MIT

# WICHTIG: Die Zendure App muss vorher einmalig mit dem Account verbunden werden
#          Sobald sich mit diesem Script verbunden wird, wird die Verbindung in der App getrennt
#          und kann nur wieder hergestellt werden, wenn das Passwort in der App neu eingegeben wird
#          Vllt. kann das jemand mal mit einem gesharten Account testen

# Wie man die Box dann mit den gewonnen Informationen fernsteuern kann, zeige ich in einem
# weiteren Script unter Verwendung der MQTT-API und der ZendureShell (PowerShell Modul mit MQTT-Client)

# Benutzername eures Zendure Accounts in der App
$appUsername = "xxxxxxxxxxxx@xxx.xx";
$appPassword = "xxxxxxxxxxxxxxxxx";

$hostname = "app.zendure.tech/eu";

$solarFlowAuthPath          = "auth/app/token";
$solarFlowDetailPath        = "device/solarFlow/detail";
$solarFlowDeviceListPath    = "productModule/device/queryDeviceListByConsumerId";

$solarFlowAuthUrl           = "https://$hostname/$solarFlowAuthPath";
$solarFlowDeviceListUrl     = "https://$hostname/$solarFlowDeviceListPath";
$solarFlowDetailUrl         = "https://$hostname/$solarFlowDetailPath";

$authenticationHeader = @{
    'Content-Type'          = 'application/json'
    'Accept-Language'       = 'de-DE'
    'appVersion'            = '4.3.1'
    'User-Agent'            = 'Zendure/4.3.1 (iPhone; iOS 14.4.2; Scale/3.00)'
    'Accept'                = '*/*'
    'Authorization'         = 'Basic Q29uc3VtZXJBcHA6NX4qUmRuTnJATWg0WjEyMw=='
    'Blade-Auth'            = 'bearer (null)'
}

# bin mir nicht sicher ob die appId irgendeinen Sinn ergibt
$authenticationBody = @{
    'password'              = "$appPassword"
    'account'               = "$appUsername"
    'appId'                 = '121c83f761305d6cf7e'
    'appType'               = 'iOS'
    'grantType'             = 'password'
    'tenantId'              = ''
}

$authenticationBodyJson = $authenticationBody | ConvertTo-Json

$response = Invoke-WebRequest -Uri $solarFlowAuthUrl -Method Post -Body $authenticationBodyJson -Headers $authenticationHeader -SessionVariable session 

#
# Debug-Ausgabe
# $response.RawContent
#

# Das Bearer-Token wird für die weitere Kommunikation benötigt
# Diese wird dann später benutzt um die SolarFlow Box zu steuern
$bearerToken = $response.Content | ConvertFrom-Json | Select-Object -ExpandProperty data |  Select-Object -ExpandProperty accessToken
$session.Headers["Blade-Auth"] = "bearer $bearerToken";
$bearerToken

# Laden der Geräteliste
# Die Geräteliste wird benötigt um die ID der Geräte zu ermitteln
$responseDeviceList = @();
$responseDeviceList = (Invoke-WebRequest -Uri $solarFlowDeviceListUrl -Method Post -Headers $session.Headers).Content | ConvertFrom-Json | Select-Object -ExpandProperty data

# In meinem Fall möchte ich nur das erste Gerät auslesen
$firstDeviceId = $responseDeviceList[0].id
$deviceIdQueryBody = @{ 'deviceId' = "$firstDeviceId" }
$deviceIdQueryBodyJson = $deviceIdQueryBody | ConvertTo-Json;

# Laden der Detaildaten des Gerätes
$responseDetails = Invoke-WebRequest -Uri $solarFlowDetailUrl -Method Post -Body $deviceIdQueryBodyJson  -Headers $session.Headers

# SolarFlow Box Daten
$solarFlowBoxData = @{
        Box         = $($responseDetails | ConvertFrom-Json | Select-Object -ExpandProperty data | Select-Object -ExcludeProperty packDataList) 
        Batterien   = $($responseDetails | ConvertFrom-Json | Select-Object -ExpandProperty data | Select-Object -ExpandProperty packDataList)
}

# Debug-Ausgabe
$solarFlowBoxData.Box
$solarFlowBoxData.Batterien

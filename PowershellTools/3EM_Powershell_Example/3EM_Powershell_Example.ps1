<#
 # Author: Benjamin Machuletz
 # Date: 2023-11.08
 # Version: 0.1
 # Description: use ZendureShellShared to control the SolarFlow Box and realize zero power supply with Shelly EM3 Pro
 #>

$FolderToExample = "C:\Users\bmach\Documents\GitHub\ZendureShell\PowershellTools\3EM_Powershell_Example";

Import-Module "$FolderToExample\ZendureShellShared.dll";
[System.Reflection.Assembly]::LoadFrom("$FolderToExample\LiteDB.dll")
[System.Reflection.Assembly]::LoadFrom("$FolderToExample\MQTTnet.dll")


$pro3EmIp  = "192.168.178.187";
$pro3EmUrl = "http://$pro3EmIp/rpc/EM.GetStatus?id=0";

$hardOutputLimit = 600;

$config = [ZendureShellShared.FileHandler]::LoadConfigFromAppData();

$apiWrapper = New-Object ZendureShellShared.ZendureApiWrapper($config.AccountName, $config.Password, $config.BearerToken);
$loginResult = $apiWrapper.Login().Result.data;
$selectedSolarFlowHub = $loginResult | Where-Object { $_.productName -eq "SolarFlow2.0" };

$mqttClient = New-Object ZendureShellShared.ZendureMqttClientProd($null);


while($true)
{
    [int]$totalPowerCurrent = ((Invoke-WebRequest -Uri $pro3EmUrl -Method Get).Content | ConvertFrom-Json).total_act_power;
    $SolarFlowHub    = $apiWrapper.GetDeviceDetails($selectedSolarFlowHub.id).Result.data;

    Write-Host "TotalPowerCurrent: $totalPowerCurrent";
    Write-Host "OutputLimit: $($SolarFlowHub.outputLimit)";
    Write-Host "OutputHomePower: $($SolarFlowHub.outputHomePower)";

    if($totalPowerCurrent -ge 20 -and $SolarFlowHub.outputLimit - $SolarFlowHub.outputHomePower -lt 5)
    {
    
        $newOutputLimit  = $SolarFlowHub.outputLimit + $totalPowerCurrent;

        if($newOutputLimit -le $hardOutputLimit)
        {
            $mqttClient.Publish("iot/$($SolarFlowHub.productKey)/$($SolarFlowHub.deviceKey)/properties/write", "{`"properties`":{`"outputLimit`":$newOutputLimit}}");
        
            Write-Host "Setze OutputLimit auf $newOutputLimit";
        }

    }
    elseif ($totalPowerCurrent -le -20 -and $SolarFlowHub.outputLimit - $SolarFlowHub.outputHomePower -lt 30)
    {

        $newOutputLimit  = $totalPowerCurrent + $SolarFlowHub.outputLimit;

        if($newOutputLimit -lt 0)
        {
            $newOutputLimit = 0;
        }
        if($newOutputLimit -le $hardOutputLimit)
        {
            $mqttClient.Publish("iot/$($SolarFlowHub.productKey)/$($SolarFlowHub.deviceKey)/properties/write", "{`"properties`":{`"outputLimit`":$newOutputLimit}}");

            Write-Host "Setze OutputLimit auf $newOutputLimit";
        }
    }

    Start-Sleep -Seconds 10;
}

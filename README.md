# ZendureShell
 Powershell-Modul für die Steuereung von SolarFlow <br />
 (Modul ist noch nicht eingecheckt. Hier aber schon einmal ein paar Erkenntnisse aus meiner Arbeit)

# [Developer-Api Login](PowershellTools/GetDeveloper.ps1) <br />
# [Nachbau App-Loging](PowershellTools/ZendureApp_LoginAndGetData.ps1) <br /><br />


# ZendureCmd Example-Commands
Wenn noch keine Anmeldeinformationen eingegeben wurden, darf kann man auch direkt mit "ZendureCmd.exe --getDeviceList" beginnen. Anschließend wird man zu Eingabe der Benutzerinformationen gebeten (derzeit wird das Kennwort in PlainText auf die Konsole gedruckt - wird noch gefixt).
## Anmelden und speichern der Anmeldeinformationen
Dezeit werden die Anmeldeinformationen im Klartext in %APPDAT%\ZendureCmd gespeichert.
<br />
```console
ZendureCmd.exe --accountname "xxxxxx@xxxxxxxxxx.xx" --password "xxxxxxx"
```
## Geräteliste abfragen
```console
ZendureCmd.exe --getDeviceList
```
## Details zu einem Gerät abfragen
```console
ZendureCmd.exe --getDeviceDetails <DeviceIdSolarflowHub or DeviceIdShell3EmPro>
```
## Details zu den Akkus abfragen
```console
ZendureCmd.exe --getBatteryDetails <DeviceIdBattery>
```
## Deverloper MQTT-Accountinformationen abrufen
```console
ZendureCmd.exe --getDeveloperAccess --accountname="xxxxxx@xxxxxxxxxx.xx" --serial="SERIENNUMMER_GERÄT"
```

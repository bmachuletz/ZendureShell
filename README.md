# ZendureShell
 Powershell-Modul für die Steuereung von SolarFlow <br />
 (Modul ist noch nicht eingecheckt. Hier aber schon einmal ein paar Erkenntnisse aus meiner Arbeit)

# [Developer-Api Login](PowershellTools/GetDeveloper.ps1) <br />
# [Nachbau App-Loging](PowershellTools/ZendureApp_LoginAndGetData.ps1) <br /><br />


# ZendureCmd Example-Commands
## Anmelden und speichern der Anmeldeinformationen
Dezeit werden die Anmeldeinformationen im Klartext in %APPDAT%\ZendureCmd gespeichert.
<br />
```console
ZendureCmd.exe --accountname "xxxxxx@xxxxxxxxxx.xx" --password "xxxxxxx"
```
## Geräteliste abfragen
Dezeit werden die Anmeldeinformationen im Klartext in %APPDAT%\ZendureCmd gespeichert.
<br />
```console
ZendureCmd.exe --getDeviceList
```
## Details zu einem Gerät abfragen
Dezeit werden die Anmeldeinformationen im Klartext in %APPDAT%\ZendureCmd gespeichert.
<br />
```console
ZendureCmd.exe --getDeviceDetails <DeviceId>
```
<br /><br />
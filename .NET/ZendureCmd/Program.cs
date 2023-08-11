using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Threading.Tasks;
using ZendureShellShared;

namespace ZendureCmd
{
    public static class ZendureCmd
    {
        private static string _accountname = string.Empty;
        private static string _password = string.Empty;
        private static string _authToken = string.Empty;
        private static bool _getDeviceList = false;
        private static string _getDeviceDetails = string.Empty;
        private static bool _getDeveloperAccess = false;
        private static string _getBatteryDetails = string.Empty;
        private static bool _reauth = false;

        private static bool credentialsFromConfigFile = false;
        public static async Task Main(string[] args)
        {
            ZendureCredentials credentials = new ZendureCredentials();
            await credentials.Fill();

            ZendureApiWrapper zendureHttp;

            var rootCommand = new RootCommand
            {
                new Option<string>("--accountname", "Anmeldename in der Zendure-APP.") { IsRequired = false },
                new Option<string>("--password", "Kennwort in der Zendure-APP.") { IsRequired = false },
                new Option<string>("--serial", "Seriennummer des Geräts.") { IsRequired = false },
                new Option<bool>("--activateDeviceControl", "Aktiviert die Steuerung des Gerätes.") { IsRequired = false },
                new Option<bool>("--getDeviceList", "Gibt eine Liste aller Geräte zurück.") { IsRequired = false },
                new Option<string>("--getDeviceDetails", "Gibt Details zu einem Gerät zurück.") { IsRequired = false },
                new Option<bool>("--getDeveloperAccess", "Daten für den MQTT-Developerzugang anzeigen.") { IsRequired = false },
                new Option<string>("--getBatteryDetails", "Batterieladung anzeigen.") { IsRequired = false },
                new Option<bool>("--reauth", "Neuanmeldung an der RestAPI (bspw. Wechsel des Accounts).") { IsRequired = false }

            };

            rootCommand.Handler = CommandHandler.Create<string, string, string, bool, bool, string, bool, string, bool>(
                async  (accountname, password, serial, activateDeviceControl, getDeviceList, 
                        getDeviceDetails, getDeveloperAccess, getBatteryDetails, reauth) =>
            {
                if(getDeveloperAccess)
                {
                    if(string.IsNullOrEmpty(serial) || string.IsNullOrEmpty(accountname))
                    {
                        Console.WriteLine("Accountname und Seriennummer des Geräts müssen angeben werden.");
                        Environment.Exit(-1);
                    }
                    else if(!string.IsNullOrEmpty(credentials.AccountName) || !string.IsNullOrEmpty(credentials.AccountName))
                    {
                        _getDeveloperAccess = true;
                        _accountname = credentials.AccountName;
                        _password = credentials.Password;
                    }
                    else
                    {
                        _getDeveloperAccess = true;
                        _accountname = accountname;
                        _password = serial;
                    }
                    return;
                }
                

                if (!string.IsNullOrEmpty(credentials.AccountName) && !string.IsNullOrEmpty(credentials.Password) && !string.IsNullOrEmpty(credentials.BearerToken))
                {
                    _accountname = credentials.AccountName;
                    _password = credentials.Password;
                    _authToken = credentials.BearerToken;

                    credentialsFromConfigFile = true;
                }

                if (getDeviceList == true)
                {
                    _getDeviceList = true;
                }

                if (getDeviceDetails != null)
                {
                    _getDeviceDetails = getDeviceDetails;
                }

                if (getBatteryDetails != null)
                {
                    _getBatteryDetails = getBatteryDetails;
                }

                // DEFAULT equals false
                _reauth = reauth;

                if (activateDeviceControl)
                {

                }

            });
            await rootCommand.InvokeAsync(args);
            
            if(_reauth == true)
            {
                await credentials.Fill(true);

                return;
            }

            if(_getDeveloperAccess == true)
            {
                ZendureApiWrapper zendureHttpForDeveloperAccess = new ZendureApiWrapper(_password, _accountname);
                var developerData = await zendureHttpForDeveloperAccess.GetDeveloperToken();
                Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{developerData.DataToJson()}{Environment.NewLine}");
                Environment.Exit(-1);
            }
            else if(string.IsNullOrEmpty(_accountname) || string.IsNullOrEmpty(_password))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\nKeine Anmeldedaten gefunden.");
                Console.ResetColor();

                Environment.Exit(-1);
            }

            if(string.IsNullOrEmpty(_authToken))
            {
                zendureHttp = new ZendureApiWrapper(_accountname, _password, string.Empty);
            }
            else
            {
                zendureHttp = new ZendureApiWrapper(_accountname, _password, _authToken);
            }
            

            var loginResponse = await zendureHttp.Login();

            if(zendureHttp.LoggedIn == true)
            {
                if (credentialsFromConfigFile == false)
                {
                    ZendureAuthFile loginData = new ZendureAuthFile
                    {
                        AccountName = _accountname,
                        Password = _password,
                        BladeAuth = $"{((ZendureAuthResponse)loginResponse).data.accessToken}"
                    };
                    /*
                    ZendureShellShared.FileHandler.SaveFileInAppData(
                        "RestApi_Config.json",
                        JsonConvert.SerializeObject(loginData));
                    */
                }
            }
            else
            {
                Console.WriteLine("Fehler beim Einloggen.");
            }

            if(zendureHttp.LoggedIn == true && _getDeviceList == true)
            {
                var deviceListResponse = await zendureHttp.GetDeviceList();
                Console.WriteLine(JsonConvert.SerializeObject(deviceListResponse));
            }
            else if(zendureHttp.LoggedIn == true && _getDeviceDetails != string.Empty)
            {
                var deviceDetailsResponse = await zendureHttp.GetDeviceDetails(_getDeviceDetails);
                Console.WriteLine(JsonConvert.SerializeObject(deviceDetailsResponse));
            }
            else if(zendureHttp.LoggedIn == true && _getBatteryDetails != string.Empty)
            {
                Console.WriteLine(Environment.NewLine);
                var deviceDetailsResponse = await zendureHttp.GetDeviceDetails(_getBatteryDetails) as ZendureDeviceDetailsResponse;
                deviceDetailsResponse.data.packDataList.ForEach(async device =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(device));
                    Console.WriteLine(Environment.NewLine);
                });
            }
            else
            {
                Console.WriteLine("Keine Aktion ausgewählt.");
            }
        }
    }
}


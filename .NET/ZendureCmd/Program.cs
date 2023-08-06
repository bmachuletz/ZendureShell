using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Runtime.CompilerServices;
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

        private static bool credentialsFromConfigFile = false;
        public static async Task Main(string[] args)
        {
            ZendureApiWrapper zendureHttp;

            var rootCommand = new RootCommand
            {
                new Option<string>("--accountname", "Anmeldename in der Zendure-APP.") { IsRequired = false },
                new Option<string>("--password", "Kennwort in der Zendure-APP.") { IsRequired = false },
                new Option<bool>("--activateDeviceControl", "Aktiviert die Steuerung des Gerätes.") { IsRequired = false },
                new Option<bool>("--getDeviceList", "Gibt eine Liste aller Geräte zurück.") { IsRequired = false },
                new Option<string>("--getDeviceDetails", "Gibt Details zu einem Gerät zurück.") { IsRequired = false }
            };

            rootCommand.Handler = CommandHandler.Create<string, string, bool, bool, string>((accountname, password, activateDeviceControl, getDeviceList, getDeviceDetails) =>
            {
                if (!string.IsNullOrEmpty(accountname) && !string.IsNullOrEmpty(password))
                {
                    _accountname = accountname;
                    _password = password;
                    // Console.WriteLine($"Accountname: {accountname}");
                }
                else
                {
                    ZendureAuthFile loginData = JsonConvert.DeserializeObject<ZendureAuthFile>(ZendureShellShared.FileHandler.LoadFileFromAppData("ZendureLoginInformation.json"));

                    if (loginData != null)
                    {
                        _accountname = loginData.AccountName;
                        _password = loginData.Password;
                        _authToken = loginData.BladeAuth;
                    }

                    credentialsFromConfigFile = true;
                }

                if (getDeviceList == true)
                {
                    // Console.WriteLine("GetDeviceList: true");
                    _getDeviceList = true;
                }

                if (getDeviceDetails != null)
                {
                    // Console.WriteLine($"GetDeviceDetails: {getDeviceDetails}");
                    _getDeviceDetails = getDeviceDetails;
                }

                if (activateDeviceControl != null)
                {
                    // Console.WriteLine($"ActivateDeviceControl: {activateDeviceControl}");
                }

            });
            rootCommand.Invoke(args);
            
            if(string.IsNullOrEmpty(_accountname) || string.IsNullOrEmpty(_password))
            {
                Console.WriteLine("\r\nKeine Anmeldedaten gefunden.");
              //  throw new Exception("Keine Anmeldedaten gefunden.");

                Environment.Exit(-1);
            }

            if(string.IsNullOrEmpty(_authToken))
            {
                zendureHttp = new ZendureApiWrapper(_accountname, _password);
            }
            else
            {
                zendureHttp = new ZendureApiWrapper(_accountname, _password, _authToken);
            }
            

            var loginResponse = await zendureHttp.Login();

            if(zendureHttp.LoggedIn == true)
            {
                // Console.WriteLine("Erfolgreich eingeloggt.");

                if (credentialsFromConfigFile == false)
                {
                    ZendureAuthFile loginData = new ZendureAuthFile
                    {
                        AccountName = _accountname,
                        Password = _password,
                        BladeAuth = $"bearer {((ZendureShellShared.ZendureAuthResponse)loginResponse).data.accessToken}"
                    };

                    ZendureShellShared.FileHandler.SaveFileInAppData(
                        "ZendureLoginInformation.json",
                        JsonConvert.SerializeObject(loginData));
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
            else
            {
                Console.WriteLine("Keine Aktion ausgewählt.");
            }
        }
    }
}


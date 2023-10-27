using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    
    public class ZendureCredentials
    {
        public Int32 Id { get; set; }
        private bool needToBeSaved = false;
        private string? _accountname  = string.Empty, _password = string.Empty;
        private string _serialNumber = string.Empty, _appKey   = string.Empty;
        private string _clientSecret = string.Empty, _bearerToken = string.Empty;

        private string filePath = string.Empty;
        private string? configurationJson = string.Empty;

        private List<string> _deviceKeys = new List<string>();
        public string AppKey { get { return _appKey; } } 
        public List<string> DeviceKeys { get { return _deviceKeys; } }
        public string ClientSecret { get { return _clientSecret; } }
        public string SerialNumber { get { return _serialNumber; } set { _serialNumber = value; } }
        public string? AccountName { get { return _accountname; } set { _accountname = value; } }
        public string? Password { get { return _password; } set { _password = value; } }
        public string BearerToken { get { return _bearerToken; } set { _bearerToken = value; } }

        ZendureApiWrapper restApi;
        ZendureCredentials data;

        public ZendureCredentials()
        {
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string programName = "ZendureCmd";
            string programFolderPath = Path.Combine(appDataFolderPath, programName);

            //  filePath = Path.Combine(programFolderPath, "keyData.json");
            filePath = Path.Combine(programFolderPath, "ZendureConfig.dat");
        }

        public async Task<bool> AuthenticateRestApi(string AccountName = "", string Password = "")
        {
            if(string.IsNullOrEmpty(AccountName))
            {
                Console.Write("Bitte geben Sie den Bentuzernamen aus der Zendure-App ein: ");
                _accountname = Console.ReadLine();
            }
            else
            {
                _accountname = AccountName;
            }

            if (string.IsNullOrEmpty(Password))
            {
                Console.Write("Bitte geben Sie das Kennwort aus der Zendure-App an: ");
                _password = Console.ReadLine();
            }
            else
            {
                _password = Password;
            }


            if (!string.IsNullOrEmpty(_accountname) || !string.IsNullOrEmpty(_password))
            {
                restApi = new ZendureApiWrapper(_accountname, _password, "");
                var authResponse = await restApi.Login() as ZendureAuthResponse;

                if (authResponse != null && restApi.LoggedIn == true)
                {
                    
                    _bearerToken = $"bearer {(authResponse).data.accessToken}";
                    
                    ZendureStatics.AUTH_HEADER["Authorization"] = _bearerToken;
                }

                return restApi.LoggedIn;
            }

            return false;
        }

        public async Task Fill(bool Reauthenticate = false)
        {
            if (!File.Exists(filePath))
            {
                needToBeSaved = true;
             
                Console.WriteLine("Es wurde keine Konfigurationsdatei gefunden.");
                Console.WriteLine("Für die erste Anmeldung müssen Sie ihr primäres Konto verwenden.");

                if (await AuthenticateRestApi() == true)
                {
// restApi = new ZendureApiWrapper(_accountname, _password, "");
                    var deviceListResponse = await restApi.GetDeviceList() as ZendureDeviceListResponse;

                    if (deviceListResponse != null)
                    {
                        _serialNumber = deviceListResponse.data[0].snNumber;
                        _deviceKeys.Add(deviceListResponse.data[0].deviceKey);

                        ZendureApiWrapper api2 = new ZendureApiWrapper(_serialNumber, _accountname);
                        var authResponse2 = await api2.GetDeveloperToken() as ZendureDeveloperApiResponse;

                        if (authResponse2 != null)
                        {
                            _appKey = ((ZendureDeveloperApiResponse)authResponse2).data.appKey;
                            _clientSecret = ((ZendureDeveloperApiResponse)authResponse2).data.secret;

                          //  string keyDataString = JsonConvert.SerializeObject(this);
                          //  FileHandler.SaveFileInAppData("keyData.json", keyDataString);

                            FileHandler.SaveConfigToAppData(this);
                        }

                        //configurationJson = FileHandler.LoadFileFromAppData("keyData.json");
                    }
                    else
                    {
                        Console.WriteLine("Fehler beim Abrufen der Geräteliste. Bitte versuchen Sie es erneut.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Fehler beim Login. Bitte versuchen Sie es erneut.");
                    return;
                }
            }
            else
            {
                if(Reauthenticate == true)
                {
                    needToBeSaved = true;

                    /*
                    configurationJson = FileHandler.LoadFileFromAppData("keyData.json");
                    data = JsonConvert.DeserializeObject<ZendureCredentials>(configurationJson);
                    */
                    data = FileHandler.LoadConfigFromAppData();

                    Console.WriteLine("Wechsel des Kontos für die REST-API.");

                    if(await AuthenticateRestApi() == true)
                    {
                        data.AccountName = _accountname;
                        data.Password = _password;
                        data.BearerToken = _bearerToken;

                        FileHandler.SaveConfigToAppData(data);
                    }

                   // configurationJson = JsonConvert.SerializeObject(data);


                }
                else
                {
                    data = FileHandler.LoadConfigFromAppData();
                    //configurationJson = FileHandler.LoadFileFromAppData("keyData.json");
                }


                if (data == null)
                {
                    return;
                }
            }

            if (data == null)
            {
                data = FileHandler.LoadConfigFromAppData();
                //data = JsonConvert.DeserializeObject<ZendureCredentials>(configurationJson);
            }

            _accountname = data.AccountName;
            _password = data.Password;
            _serialNumber = data.SerialNumber;
            _appKey = data.AppKey;
            _clientSecret = data.ClientSecret;
            _deviceKeys = data.DeviceKeys;
            _bearerToken = data.BearerToken;

            if (needToBeSaved == true)
            {
                FileHandler.SaveConfigToAppData(this);
                // FileHandler.SaveFileInAppData("keyData.json", configurationJson);
            }
        }
    }
}

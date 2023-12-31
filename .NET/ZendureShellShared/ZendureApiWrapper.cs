﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public class ZendureApiWrapper : ZendureHttp
    {
        private string password = string.Empty;
        private string username = string.Empty;
        private string authToken = string.Empty;

        private bool loggedIn = false;

        public bool LoggedIn { get => loggedIn; }

        private bool developerMode = false;

      //  public string Password { set => password = value; }
      //  public string Username { set => username = value; }

        public ZendureApiWrapper(string Username, string Password, string AuthToken = "")
        {
            this.password = Password;
            this.username = Username;
            this.authToken = AuthToken;

            ZendureStatics.AUTH_BODY["account"] = this.username;
            ZendureStatics.AUTH_BODY["password"] = this.password;
            ZendureStatics.AUTH_HEADER["Blade-Auth"] = this.authToken;
        }

        public ZendureApiWrapper(string SerialNumber, string Username)
        {
            developerMode = true;
            DeveloperRequestBody = new ZendureDeveloperRequestBody { snNumber = SerialNumber, account = Username };
        }

        public async Task<IZendureResponse> Login()
        {
            IZendureResponse? x = null;

            if (developerMode == true)
            {
                return new ZendureMessageResponse { Message = "Zugriff auf die REST-API im Developermode nicht verfügbar."};
            }

            if (ZendureStatics.AUTH_HEADER["Blade-Auth"].Length < 10)
            {
                x = new ZendureAuthResponse();
                x = await GetResponse(HttpMethod.Post, ZendureStatics.APP_AUTH_URL) as ZendureAuthResponse;
                if (x.success == true)
                {
                    loggedIn = true;
                }
            }
            else
            {
                loggedIn = true;
                x = new ZendureDeviceListResponse();
                x = await GetDeviceList();
                
                if (x.success == true)
                {
                    loggedIn = true;
                    Console.WriteLine(ZendureStatics.AUTH_HEADER["Blade-Auth"]);

                } 
                else
                {
                    loggedIn = false;
                    ZendureStatics.AUTH_HEADER["Blade-Auth"] = "bearer (null)";
                    await Login();
                }
            }

            return x;
        }

        public async Task<IZendureResponse> GetDeviceList(bool printDebug = false)
        {
            if (developerMode == true)
            {
                return new ZendureMessageResponse { Message = "Zugriff auf die REST-API im Developermode nicht verfügbar." };
            }

            if (loggedIn == false)
            {
                Console.WriteLine(ZendureStatics.AUTH_HEADER["Blade-Auth"]);
                throw new Exception("Not logged in");
            }

            var response = await GetResponse(HttpMethod.Post, ZendureStatics.APP_DEVICELIST_URL);

            if (printDebug == true)
            {
                Console.WriteLine(response.DataToJson());
            }
     
            return response;
        }

        public async Task<IZendureResponse> GetDeviceDetails(string deviceId, bool printDebug = false)
        {
            if (developerMode == true)
            {
                return new ZendureMessageResponse { Message = "Zugriff auf die REST-API im Developermode nicht verfügbar." };
            }

            if (loggedIn == false)
            {
                throw new Exception("Not logged in");
            }
            ZendureStatics.DEVICE_DETAIL_BODY["deviceId"] = deviceId;
            
            var response = await GetResponse(HttpMethod.Post, ZendureStatics.APP_DETAILS_URL);

            if (printDebug == true)
            {
                Console.WriteLine(response.DataToJson());
            }

            return response;
        }

        public async Task<IZendureResponse> GetDeveloperToken()
        {
            if (developerMode == false)
            {
                return await new Task<IZendureResponse>(() => { return new ZendureMessageResponse { Message = "Zugriff auf den Developermode im REST-API Modus nicht verfügbar." }; } );
            }

            return await GetResponse(HttpMethod.Post, ZendureStatics.APP_DEVELOPER_URL);
        }

        
    }
}

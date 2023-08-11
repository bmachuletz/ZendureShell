using System;
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
            if(developerMode == true)
            {
                return new ZendureMessageResponse { Message = "Zugriff auf die REST-API im Developermode nicht verfügbar."};
            }

            var x = await GetResponse(HttpMethod.Post, ZendureStatics.APP_AUTH_URL) as ZendureAuthResponse;
            if (x.success == true)
            {
                loggedIn = true;
            }

            return x;
        }

        public async Task<IZendureResponse> GetDeviceList()
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
            await GetResponse(HttpMethod.Post, ZendureStatics.APP_AUTH_URL);
            return await GetResponse(HttpMethod.Post, ZendureStatics.APP_DEVICELIST_URL);
        }

        public async Task<IZendureResponse> GetDeviceDetails(string deviceId)
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
            
            return await GetResponse(HttpMethod.Post, ZendureStatics.APP_DETAILS_URL);
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

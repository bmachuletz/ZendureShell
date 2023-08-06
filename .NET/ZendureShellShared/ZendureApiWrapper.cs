

namespace ZendureShellShared
{
    public class ZendureApiWrapper : ZendureHttp
    {
        private string password = string.Empty;
        private string username = string.Empty;
        private string authToken = string.Empty;

        private bool loggedIn = false;

        public bool LoggedIn { get => loggedIn; }

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


        public async Task<IZendureResonse> Login()
        {
          //  if (string.IsNullOrEmpty(this.authToken))
           // {
                var x = await GetResponse(HttpMethod.Post, ZendureStatics.APP_AUTH_URL) as ZendureAuthResponse;
                if (x.success == true)
                {
                    loggedIn = true;
                }

                return x;
           /* }
            else
            {
                loggedIn = true;
                var x = new ZendureAuthResponse();
                x.success = true;

                return x;
            }*/
        }

        public async Task<IZendureResonse> GetDeviceList()
        {
            if(loggedIn == false)
            {
                throw new Exception("Not logged in");
            }
            await GetResponse(HttpMethod.Post, ZendureStatics.APP_AUTH_URL);
            return await GetResponse(HttpMethod.Post, ZendureStatics.APP_DEVICELIST_URL);
        }

        public async Task<IZendureResonse> GetDeviceDetails(string deviceId)
        {
            if (loggedIn == false)
            {
                throw new Exception("Not logged in");
            }
            ZendureStatics.DEVICE_DETAIL_BODY["deviceId"] = deviceId;
            
            return await GetResponse(HttpMethod.Post, ZendureStatics.APP_DETAILS_URL);
        }
    }
}

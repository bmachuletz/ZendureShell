using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ZendureShellShared
{
    public abstract class ZendureHttp : IDisposable
    {
        private HttpClient? client;
        private HttpClientHandler handler;
        private ZendureDeveloperRequestBody developerReuestBody;
        private string body;
        dynamic? authResponse;

        public ZendureDeveloperRequestBody DeveloperRequestBody { get => developerReuestBody; set => developerReuestBody = value; }

        public ZendureHttp()
        {
            var cookieContainer = new CookieContainer();
            handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            client = new HttpClient(handler);
        }

        protected async Task<IZendureResponse> GetResponse(HttpMethod method, string Url)
        {
            string returnBody = string.Empty;

            try
            {
                HttpResponseMessage response;
                string? responseBody = string.Empty;
      
                StringContent data;
                client.DefaultRequestHeaders.Clear();

                switch (method.Method)
                {
                    // GET is currently not used
                    case "GET":
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        response = await client.GetAsync(Url);
                        response.EnsureSuccessStatusCode();
                        responseBody = await response.Content.ReadAsStringAsync();
                        break;

                    case "POST":
                        if (Url.Equals(ZendureStatics.APP_AUTH_URL))
                        {

                            authResponse = new ZendureAuthResponse();
                            body = JsonConvert.SerializeObject(ZendureStatics.AUTH_BODY);
                            data = new StringContent(body, Encoding.UTF8, "application/json");

                        }
                        else if (Url.Equals(ZendureStatics.APP_DEVICELIST_URL))
                        {
                            authResponse = new ZendureDeviceListResponse();
                            data = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                        }
                        else if (Url.Equals(ZendureStatics.APP_DETAILS_URL))
                        {
                            authResponse = new ZendureDeviceDetailsResponse();
                            body = JsonConvert.SerializeObject(ZendureStatics.DEVICE_DETAIL_BODY);
                            data = new StringContent(body, Encoding.UTF8, "application/json");

                        }
                        else if (Url.Equals(ZendureStatics.APP_DEVELOPER_URL))
                        {
                            authResponse = new ZendureDeveloperApiResponse();
                            body = JsonConvert.SerializeObject(new ZendureDeveloperRequestBody { account = developerReuestBody.account, snNumber = developerReuestBody.snNumber });
                            data = new StringContent(body, Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            throw Exception("Unknown Url");
                        }

                        if (!Url.Equals(ZendureStatics.APP_DEVELOPER_URL))
                        {
                            foreach (KeyValuePair<string, string> header in ZendureStatics.AUTH_HEADER)
                            {
                                client.DefaultRequestHeaders.Add(header.Key, header.Value);
                            }
                        }

                        response = await client.PostAsync(Url, data);
                        response.EnsureSuccessStatusCode();
                        responseBody = await response.Content.ReadAsStringAsync();

                        if (response.RequestMessage.RequestUri != null && responseBody != null)
                        {
                            if (response.RequestMessage.RequestUri.ToString().Equals(ZendureShellShared.ZendureStatics.APP_AUTH_URL))
                            {
                                authResponse = JsonConvert.DeserializeObject<ZendureAuthResponse>(responseBody);
                                ZendureStatics.AUTH_HEADER["Blade-Auth"] = $"bearer {authResponse.data.accessToken}";
                                client.DefaultRequestHeaders.Remove("Blade-Auth");
                                client.DefaultRequestHeaders.Add("Blade-Auth", ZendureStatics.AUTH_HEADER["Blade-Auth"]);
                                returnBody = "{ \"status\": \"OK\" }";
                                Console.WriteLine(authResponse.data.accessToken);
                            }
                            else if (response.RequestMessage.RequestUri.ToString().Equals(ZendureShellShared.ZendureStatics.APP_DEVICELIST_URL))
                            {
                                authResponse = JsonConvert.DeserializeObject<ZendureDeviceListResponse>(responseBody);
                                if (authResponse != null)
                                {
                                    returnBody = JsonConvert.SerializeObject(authResponse.data);
                                }

                            }
                            else if (response.RequestMessage.RequestUri.ToString().Equals(ZendureShellShared.ZendureStatics.APP_DETAILS_URL))
                            {
                                var settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };
                                authResponse = JsonConvert.DeserializeObject<ZendureDeviceDetailsResponse>(responseBody, settings);
                                if (authResponse != null)
                                {
                                    returnBody = JsonConvert.SerializeObject(authResponse.data);
                                }
                            }
                            else if (response.RequestMessage.RequestUri.ToString().Equals(ZendureShellShared.ZendureStatics.APP_DEVELOPER_URL))
                            {
                                authResponse = JsonConvert.DeserializeObject<ZendureDeveloperApiResponse>(responseBody);
                                if (authResponse != null)
                                {
                                    
                                    returnBody = authResponse.DataToJson();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unknown URL");
                            }
                            //     Console.WriteLine(responseBody);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return authResponse;
        }

        private Exception Exception(string v)
        {
            Console.WriteLine(v);
            return new Exception(v);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
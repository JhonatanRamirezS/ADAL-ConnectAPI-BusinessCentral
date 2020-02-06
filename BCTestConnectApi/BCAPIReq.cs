using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using Newtonsoft.Json;
using AuthenticationContext = Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System.IO;

namespace BCTestConnectApi
{
    class BCAPIReq
    {
        const string ClientRedirectUrl = "https://localhost";
        static string clientId = "APPID";
        static string clientSecret = "SECRET APP KEY";
        static string accessToken = null;
        static string resource = "https://api.businesscentral.dynamics.com";
        static string bcResponse;
        static string authorityUriv2 = "https://login.microsoftonline.com/e53d2a43-6254-40d3-bf7e-6cb773a25285";
        
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            
            ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);
            var authenticationContext = new AuthenticationContext(authorityUriv2, true);
            var authContext = new AuthenticationContext(authorityUriv2);
            var authenticationResult = authContext.AcquireTokenAsync(resource, clientId, new Uri(ClientRedirectUrl), new PlatformParameters(PromptBehavior.Auto)).GetAwaiter().GetResult();
            accessToken = authenticationResult.AccessToken;

            //var clientBC = new RestClient("https://api.businesscentral.dynamics.com/v2.0/Production/api/CertTech/Integrations/v1.0");
            var clientBC = new RestClient("https://api.businesscentral.dynamics.com/v2.0/Production/api/CertTech/Integrations/v1.0/companies");
            clientBC.Timeout = -1;
            var requestBC = new RestRequest(Method.GET);
            requestBC.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse responseBC = clientBC.Execute(requestBC);
            bcResponse = responseBC.Content;
            object resposeDataBC = JsonConvert.DeserializeObject(bcResponse);

            Console.Write(resposeDataBC);
            Console.ReadKey();
        }
        public static string HttpGet(string URI)
        {
            string html = string.Empty;
            
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                HttpResponseMessage response = client.GetAsync(new Uri(URI)).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result);
            }

            return html;
        }

        public static async Task Auth()
        {
            using (HttpClient httpclient = new HttpClient())
            {
                httpclient.Timeout = new TimeSpan(0, 2, 0);
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await httpclient.GetAsync("https://api.businesscentral.dynamics.com/v2.0/Production/api/CertTech/Integrations/v1.0/companies");
            }
        }
    }
}

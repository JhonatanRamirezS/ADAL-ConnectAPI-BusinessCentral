using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;


namespace BCTestConnectApi
{
    class Program
    {
        // Azure AD registrations:
        // Specifies the Azure AD tenant ID
        const string AadTenantId = "TurnkeyTec.onmicrosoft.com";
        static string clientId = "d7f6b8e1-b3a0-4454-b4bb-fb8de6a3e2b5";
        static string clientSecret = "=_X@gsLAvoRNQR?WsvSASrDMjx4c6G82";

        // The server base address
        //static string baseUrl = "https://login.windows.net/e53d2a43-6254-40d3-bf7e-6cb773a25285/oauth2/authorize?resource=https://api.businesscentral.dynamics.com";
        static string baseUrl = "https://login.windows.net/e53d2a43-6254-40d3-bf7e-6cb773a25285/oauth2/token?resource=https://api.businesscentral.dynamics.com";
        //static string baseUrl = "https://login.windows.net/e53d2a43-6254-40d3-bf7e-6cb773a25285/oauth2/";

        // this will hold the Access Token returned from the server.
        static string accessToken = null;

        //static void Main(string[] args)
        //{
        //    try
        //    {
        //        //Console.WriteLine("Starting ...");
        //        //var clientTk = new RestClient(baseUrl);
        //        //clientTk.Timeout = -1;
        //        //var requestTk = new RestRequest(Method.POST);
        //        //IRestResponse responseTk = clientTk.Execute(requestTk);



        //        DoIt().Wait();
        //        var client = new RestClient("https://api.businesscentral.dynamics.com/v2.0/Development/api/CertTech/Integrations/v1.0");
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("Authorization", "Bearer " + accessToken);
        //        IRestResponse response = client.Execute(request);
        //        Console.WriteLine(accessToken);
        //        //Console.ReadLine();
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        private static async Task<int> DoIt()
        {
            // Get the Access Token.
            accessToken = await GetAccessToken();
            Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");
            return 0;
        }
        /// <summary>
        /// This method uses the OAuth Client Credentials Flow to get an Access Token to provide
        /// Authorization to the APIs.
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetAccessToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                // We want the response to be JSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Build up the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                postData.Add(new KeyValuePair<string, string>("redirect_uri", "https://businesscentral.dynamics.com"));
                postData.Add(new KeyValuePair<string, string>("client_id", clientId));
                postData.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
                postData.Add(new KeyValuePair<string, string>("username", "jhramirez@turnkeytec.com"));
                postData.Add(new KeyValuePair<string, string>("password", "Welcome!TK"));

                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                // Post to the Server and parse the response.
                HttpResponseMessage response = await client.PostAsync("token", content);
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);

                // return the Access Token.
                return ((dynamic)responseData).access_token;
            }
        }
    }
}

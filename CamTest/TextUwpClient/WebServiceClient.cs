using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace TextUwpClient
{
    public class WebServiceClient
    {
        private static readonly Lazy<WebServiceClient> Lazy = 
            new Lazy<WebServiceClient>(()=>new WebServiceClient());

        private WebServiceClient()
        {
            Client = new RestClient("http://videowebservice20170629020126.azurewebsites.net/");
        }

        public static WebServiceClient Instance => Lazy.Value;

        public RestClient Client { get; set; }

        public async Task<bool> PostIpData(string ip)
        {
            var request = new RestRequest("Api/Values", Method.POST);
            request.AddBody(ip);
            try
            {
                var response = await Client.Execute(request);
                return response.IsSuccess;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error on Posting IP address: " + e.Message);
                return false;
            }
        }
        public async Task<bool> UpdatePortStatus(string port, string status)
        {
            var request = new RestRequest("Api/Values/" + port, Method.PUT);
            request.AddBody(status);
            try
            {
                var response = await Client.Execute(request);
                return response.IsSuccess;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error on Updating port info: " + e.Message);
                return false;
            }
        }
    }
}

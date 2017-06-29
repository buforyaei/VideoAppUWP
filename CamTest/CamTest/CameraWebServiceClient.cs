using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CamTest
{
    public class CameraWebServiceClient
    {
        private static readonly Lazy<CameraWebServiceClient> Lazy = 
            new Lazy<CameraWebServiceClient>(()=>new CameraWebServiceClient());

        private CameraWebServiceClient()
        {
            Client = new RestClient("http://videowebservice20170629020126.azurewebsites.net/");
        }

        public static CameraWebServiceClient Instance => Lazy.Value;

        public RestClient Client { get; set; }

        public async Task<ConnectionData> GetConnectionData()
        {
            var request = new RestRequest("Api/Values", Method.GET);
            try
            {
                var response = await Client.Execute(request);
                var resultString = Encoding.UTF8.GetString(response.RawBytes, 0,
                response.RawBytes.Length);
                if (resultString.Contains("no available ports"))
                {
                    Debug.WriteLine("no available ports");
                    return null;
                }
                try
                {
                    var resultArray = (resultString.Substring(2).Replace("\"]",string.Empty).Split(new[] {':'}));
                    return new ConnectionData
                    {
                        Ip = resultArray[0],
                        Port = resultArray[1]
                    };
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error on de serializing connection data: " + e.Message);
                }
                return null;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error on getting connection data: " + e.Message);
                return null;
            }
        }
    }

    public class ConnectionData
    {
        public string Ip { get; set; }
        public string Port { get; set; }
    }
}

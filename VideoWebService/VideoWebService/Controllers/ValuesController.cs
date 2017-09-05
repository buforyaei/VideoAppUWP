using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VideoWebService.Controllers
{
    public class ValuesController : ApiController
    {
        public static string ServerIp = "";
        public static List<ServiceDataModel> Ports = new List<ServiceDataModel>();
       

        // GET api/values
        public IEnumerable<string> Get()
        {
            if (!Ports.Any()) return new[] {"no available ports"};
            var freeServices = Ports.Where(serv => serv.IsOccupied == false).ToList();
            return !freeServices.Any() ? new[] { "no available ports" } : new[] { ServerIp + ":" + freeServices.First().Port };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] object value)
        {
            ServerIp = value.ToString();
            Ports = new List<ServiceDataModel> {
                new ServiceDataModel { IsOccupied = false, Port = "40403" },
                new ServiceDataModel { IsOccupied = false, Port = "40404" },
                new ServiceDataModel { IsOccupied = false, Port = "40405" },
                new ServiceDataModel { IsOccupied = false, Port = "40406" },
                new ServiceDataModel { IsOccupied = false, Port = "40407" },
                new ServiceDataModel { IsOccupied = false, Port = "40408" },
            };
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string status)
        {
            var s = Ports.Single(service => service.Port == id.ToString());
            s.IsOccupied = status != "false";
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

using iplibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IPInfoLibraries
{

    public class IPInfoLibrary
    {
        protected string apiBase;
        protected string apiKey;

        public IPInfoLibrary(string abase, string akey)
        {
            apiBase = abase;
            apiKey = akey;
        }

        public async Task<Result<IpDetails>> GetDetails(string ip)
        {
            var c = new HttpClient();
            //c.BaseAddress = new Uri(apiBase);

            var response = await c.GetAsync($"{ip}?access_key={apiKey}");

            if (!response.IsSuccessStatusCode)
            {
                return new Result<IpDetails>(false, default(IpDetails), new Error("503", "unavailable"));
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseBody);
            if (responseJson.ContainsKey("error"))
            {
                return new Result<IpDetails>(false, default(IpDetails), new Error("500", "exception"));
            }

            var ipLookupInfo = responseJson.ToObject<IPResponse>();
            if (ipLookupInfo==null||ipLookupInfo.country_name == null )
                return new Result<IpDetails>(false, default(IpDetails), new Error("404", "not found"));

            return new Result<IpDetails>(true, new IpDetails(ipLookupInfo.Ip,ipLookupInfo.city, ipLookupInfo.country_code, ipLookupInfo.continent_code, ipLookupInfo.latitude??0, ipLookupInfo.longitude??0), null);
        }
    }
}
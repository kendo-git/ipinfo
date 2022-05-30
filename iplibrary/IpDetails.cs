using System;
namespace iplibrary
{
    public class IpDetails
    {
        public string Ip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public IpDetails(string ip, string ci, string co, string cn, double la, double lo)
        {
            Ip = ip;
            City = ci;
            Country = co;
            Continent = cn;
            Lat = la;
            Long = lo;
        }
    }
}
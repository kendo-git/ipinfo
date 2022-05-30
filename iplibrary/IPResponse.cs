using System;
using System.Collections.Generic;
using System.Text;

namespace iplibrary
{
    public class IPResponse
    {
        public string Ip { get; set; }
        public string type { get; set; }

        public string continent_code { get; set; }
        public string continent_name { get; set; }

        public string country_code { get; set; }
        public string country_name { get; set; }

        public string city { get; set; }

        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
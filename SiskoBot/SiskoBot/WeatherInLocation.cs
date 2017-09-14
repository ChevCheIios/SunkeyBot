using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiskoBot
{
    public class WeatherInLocation
    {
        public class Message
        {
            public Pogodka[] weather { get; set; }
            public MainTemp main { get; set; }
        }
        public class Pogodka
        {
            public string description { get; set; }
        }

        public class MainTemp
        {
            public string temp { get; set; }
        }
    }
}

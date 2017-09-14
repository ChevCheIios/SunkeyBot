using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiskoBot
{
   public class WeatherApi
    {
        public class Message
        {
            public Pogodka[] list { get; set; }
        }
        public class Pogodka
        {
            public string name { get; set; }
            public string id { get; set; }
            public MainTemp main { get; set; }
            public MainSys sys { get; set; }
            public MainWind wind { get; set; }
            public MainWeather[] weather { get; set; }
        }

        public class MainTemp
        {
            public string temp { get; set; }
        }
        public class MainSys
        {
            public string country { get; set; }
        }
        public class MainWind
        {
            public string speed { get; set; }
        }
        public class MainWeather
        {
            public string description { get; set; }
        }

    }
}

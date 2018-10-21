using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TigerbayFlightApp.Data
{
    public class AirportNameTranslation
    {
        Dictionary<string, string> airports = new Dictionary<string, string>();

        #region Thread safe singleton of airport names

        private static readonly AirportNameTranslation instance = new AirportNameTranslation();

        // Explicit static constructor to tell C# compiler
        static AirportNameTranslation()
        {
        }

        private AirportNameTranslation()
        {
            airports.Add("LPL", "Liverpool John Lennon Airport");
            airports.Add("EDI", "Edinburgh Airport");
            airports.Add("MAN", "Manchester Airport");
            airports.Add("STN", "London Stanstead Airport");
            airports.Add("BHX", "Birmingham International Airport");
            airports.Add("BRS", "Bristol International Airport");
            airports.Add("LBA", "Leeds Bradford Airport");
            airports.Add("BFS", "Belfast International Airport");
            airports.Add("LGW", "London Gatwick Airport");
            airports.Add("SEN", "London Southend Airport");
            airports.Add("EMA", "East Midlands Airport");
            airports.Add("NCL", "Newcastle International Airport");
            airports.Add("BOH", "Bornemouth Airport");
            airports.Add("GLA", "Glasgow International Airport");
            airports.Add("EXT", "Exeter International Airport");
            airports.Add("LTN", "London Luton Airport");
            airports.Add("CWL", "Cardiff International Airport");
            airports.Add("PIK", "Glasgow Prestwick Airport");
            airports.Add("DSA", "Doncaster Sheffield Airport");
            airports.Add("SOU", "Southampton Airport");
            airports.Add("ABZ", "Aberdeen Dyce International Airport");
            airports.Add("KRK", "Krakow John Paul II International Airport");
            airports.Add("PRG", "Vaclav Havel Airport Prague");
            airports.Add("PMI", "Palma de Mallorca Airport");
            airports.Add("IBZ", "Ibiza Airport");
            airports.Add("JER", "Jersey Airport");
            airports.Add("FAO", "Faro Airport");
            airports.Add("ALC", "Alicante International Airport");
            airports.Add("ACE", "Lanzarote Airport");
            airports.Add("MLA", "Malta International Airport");
            airports.Add("AGP", "Malaga Airport");
            airports.Add("MJV", "Murcia-San Javier Airport");
            airports.Add("FUE", "Fuertaventura Airport");
            airports.Add("LPA", "Gran Canaria Airport");
            airports.Add("LIS", "Lisbon Portela Airport");
            airports.Add("TFS", "Tenerife South Airport");
            airports.Add("FNC", "Madeira International Airport");
            airports.Add("BCN", "Barcelona El Prat International Airport");
            airports.Add("AMS", "Amsterdam Schiphol Airport");
            airports.Add("WMI", "Warsaw Modlin Airport");
            airports.Add("SXF", "Berlin Schonefeld Airport");
            airports.Add("BUD", "Budapest Liszt Ferenc International Airport");
            airports.Add("CIA", "Pastine International Airport");
            airports.Add("CDG", "Paris Charles de Gaulle Airport");
            airports.Add("BTS", "Bratislava Airport");
            airports.Add("FCO", "Leonardo Da Vinci-Fiumicino Airport");
            airports.Add("WAW", "Warsaw Chopin Airport");
        }

        public static AirportNameTranslation Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public string GetAirportName(string code)
        {
            var name = airports.ContainsKey(code) ? airports[code] : string.Empty;
            return name;
        }
    }
}
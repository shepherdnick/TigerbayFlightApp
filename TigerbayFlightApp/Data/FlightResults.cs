using System;
using System.Collections.Generic;

/// <summary>
/// Holds the classes used for deserializing data from the API calls.
/// </summary>
namespace TigerbayFlightApp.Data
{
    public class Outbound
    {
        public string originCode { get; set; }
        public string destinationCode { get; set; }
        public DateTime departs { get; set; }
        public DateTime arrives { get; set; }
        public string airlineCode { get; set; }
        public string flightNumber { get; set; }
    }

    public class Inbound
    {
        public string originCode { get; set; }
        public string destinationCode { get; set; }
        public DateTime departs { get; set; }
        public DateTime arrives { get; set; }
        public string airlineCode { get; set; }
        public string flightNumber { get; set; }
    }

    public class Flight
    {
        public int id { get; set; }
        public Outbound outbound { get; set; }
        public Inbound inbound { get; set; }
        public int adultCount { get; set; }
        public int childCount { get; set; }
        public int infantCount { get; set; }
        public double price { get; set; }
        public string currencyCode { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class FlightResults
    {
        public List<Flight> flights { get; set; }
        public List<Link> links { get; set; }
    }
}
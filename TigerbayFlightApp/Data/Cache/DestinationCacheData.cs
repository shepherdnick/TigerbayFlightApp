using System;
using System.Collections.Generic;

namespace TigerbayFlightApp.Data
{
    /// <summary>
    /// A simple class to use with the cache - expiry used to control the lifetime of the cache data
    /// </summary>
    public class DestinationCacheData
    {
        public List<string> Destinations { get; set; }
        public DateTime Expiry { get; set; }
    }
}
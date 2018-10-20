using System;
using System.Collections.Generic;

namespace TigerbayFlightApp.Data
{
    public sealed class DataCache
    {
        // Hold all the cached data in a dictionary
        private Dictionary<string, DestinationCacheData> destinations = new Dictionary<string, DestinationCacheData>();

        #region Thread safe singleton of cache layer

        private static readonly DataCache instance = new DataCache();

        // Explicit static constructor to tell C# compiler
        static DataCache()
        {
        }

        private DataCache()
        {
        }

        public static DataCache Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Returns the destinations for the given source airport
        /// </summary>
        /// <param name="sourceAirport">The source airport name</param>
        /// <returns>A list of strings</returns>
        public List<string> GetDestinations(string sourceAirport)
        {
            // Check the dictionary for a value
            var cacheData = destinations.ContainsKey(sourceAirport) ? destinations[sourceAirport] : null;

            // Check that the cache hasn't expired
            if (cacheData != null && cacheData.Expiry < DateTime.Now)
            {
                // TODO If it has, rebuild the cache
                cacheData.Destinations.Clear();
            }
            else if(cacheData == null)
            {
                // Create a blank object with an expiry, just to return the correct type
                cacheData = new DestinationCacheData()
                {
                    Destinations = new List<string>(),
                    Expiry = DateTime.Now.AddHours(6)
                };
            }

            // Return a list of destination strings
            return cacheData.Destinations;
        }

        /// <summary>
        /// Adds destination airport to source airport cache
        /// </summary>
        /// <param name="sourceAirport">The source airport</param>
        /// <param name="destinationAirport">The destination airport</param>
        public void AddDestination(string sourceAirport, string destinationAirport)
        {
            // Check the dictionary for a value
            var cacheData = destinations.ContainsKey(sourceAirport) ? destinations[sourceAirport] : null;

            if(cacheData == null)
            {
                // Create a new dictionary entry for this source airport
                destinations.Add(sourceAirport, new DestinationCacheData()
                {
                    Destinations = new List<string>() { destinationAirport },
                    Expiry = DateTime.Now.AddHours(6)
                });

                return;
            }

            // Check if the dictionary already has the destination airport
            var existingDestinationAirport = cacheData.Destinations.Contains(destinationAirport);

            if (!existingDestinationAirport)
            {
                cacheData.Destinations.Add(destinationAirport);
            }

            // Update the expiry date time
            cacheData.Expiry = DateTime.Now.AddHours(6);
        }
    }
}
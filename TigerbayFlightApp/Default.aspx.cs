﻿using System;
using System.Linq;
using TigerbayFlightApp.Data;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace TigerbayFlightApp
{
    public partial class Default : System.Web.UI.Page
    {
        private static string _baseURL = "http://tigerbaytest.azurewebsites.net/api/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                // Load the drop down menus
                GetValidDestinationCodes();
                GetValidOriginCodes();
            }            
        }

        /// <summary>
        /// Populate the destination codes drop down list
        /// </summary>
        private void GetValidDestinationCodes()
        {
            IRestClient restClient = new RestClient();
            var fullUrl = _baseURL + "GetValidDestinationCodes";
            var sourceCodes = restClient.Get<string[]>(fullUrl);

            var listItems = sourceCodes.Select(x => new ListItem(AirportNameTranslation.Instance.GetAirportName(x) + " (" + x + ")", x)).ToList();

            ddlDestinationAirport.DataTextField = "Text";
            ddlDestinationAirport.DataValueField = "Value";
            ddlDestinationAirport.DataSource = listItems;
            ddlDestinationAirport.DataBind();
        }

        /// <summary>
        /// Populate the origin codes drop down list
        /// </summary>
        private void GetValidOriginCodes()
        {
            IRestClient restClient = new RestClient();
            var fullUrl = _baseURL + "GetValidOriginCodes";
            var destinationCodes = restClient.Get<string[]>(fullUrl);
            
            var listItems = destinationCodes.Select(x => new ListItem(AirportNameTranslation.Instance.GetAirportName(x) + " (" + x + ")", x)).ToList();

            ddlSourceAirport.DataTextField = "Text";
            ddlSourceAirport.DataValueField = "Value";
            ddlSourceAirport.DataSource = listItems;
            ddlSourceAirport.DataBind();
        }

        /// <summary>
        /// Check an origin -> destination airport for flights
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">Event arguments</param>
        protected void btnCheckFlights_Click(object sender, EventArgs e)
        {
            var sourceAirport = ddlSourceAirport.SelectedValue.ToString();
            var destinationAirport = ddlDestinationAirport.SelectedValue.ToString();

            // Build the url up and then hit the API
            var fullUrl = string.Format("{0}flights/{1}/{2}/{3}", _baseURL, sourceAirport, destinationAirport, 0);

            GetAndDisplayFlightResults(fullUrl);
        }

        /// <summary>
        /// Moves the page on to the previous or the next page
        /// </summary>
        /// <param name="sender">The link button</param>
        /// <param name="e">Event argument</param>
        protected void btnPage_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            // We already have the new API request URL from the previous request
            GetAndDisplayFlightResults(e.CommandArgument.ToString());
        }

        /// <summary>
        /// Calls the API with the requested URL, and then displays the results for the flight.
        /// If no flights are found it checks all the other destinations for valid flights and adds them to the cache.
        /// </summary>
        /// <param name="url">The API URL to use</param>
        private void GetAndDisplayFlightResults(string url)
        {
            IRestClient restClient = new RestClient();

            // Get the flight results
            var flightResults = restClient.Get<FlightResults>(url);

            if (flightResults.flights.Count == 0)
            {
                // If there were no flight results, first check the cache to see if we know of alternative destinations
                var sourceAirport = ddlSourceAirport.SelectedValue.ToString();
                var existingDestinations = DataCache.Instance.GetDestinations(sourceAirport);

                if (existingDestinations.Count <= 1)
                {
                    // If there aren't any destinations, we probably need to check all available destinations
                    GetAllDestinations(sourceAirport);
                }

                // Hide the previous and next buttons
                btnNextPage.Visible = false;
                btnPreviousPage.Visible = false;

                // Build up the message to display alternative destinations when no flights were found.
                pnlError.Visible = true;
                litFlightSearchResults.Text = "<h2>Sorry, there are no flights to this destination!</h2><p>Why not try one of the following destinations instead?</p>";
                lstAlternativeDestinations.DataSource = existingDestinations;
                lstAlternativeDestinations.DataBind();
            }

            if (flightResults.links.Count > 0)
            {
                // If we found some flights, get rid of any previous search results
                pnlError.Visible = false;
                litFlightSearchResults.Text = "";
                lstAlternativeDestinations.DataSource = null;
                lstAlternativeDestinations.DataBind();

                // Get the next page links set up
                var nextPageLinks = flightResults.links.Where(x => x.rel.ToString().ToLower().Equals("nextpage")).FirstOrDefault();

                if (nextPageLinks != null)
                {
                    btnNextPage.Visible = true;
                    btnNextPage.CommandArgument = nextPageLinks.href.ToString();
                    btnNextPage.CommandName = nextPageLinks.rel.ToString();
                }
                else
                {
                    btnNextPage.Visible = false;
                }

                // Get the previous page links set up
                var previousPageLinks = flightResults.links.Where(x => x.rel.ToString().ToLower().Equals("previouspage")).FirstOrDefault();

                if(previousPageLinks != null)
                {
                    btnPreviousPage.Visible = true;
                    btnPreviousPage.CommandArgument = previousPageLinks.href.ToString();
                    btnPreviousPage.CommandName = previousPageLinks.rel.ToString();
                }
                else
                {
                    btnPreviousPage.Visible = false;
                }
            }

            // Add the flights to the search results repeater
            rptFlightSearchResults.DataSource = flightResults.flights;
            rptFlightSearchResults.DataBind();
        }

        /// <summary>
        /// Makes a request from the source airport to all the destination airports, to see if they have any flights and adds them to the cache
        /// </summary>
        /// <param name="sourceAirport">The source airport</param>
        private async void GetAllDestinations(string sourceAirport)
        {
            // Loop through all the items in the destination airports list
            foreach (ListItem item in ddlDestinationAirport.Items)
            {
                // Async-call the destination flight search
                var destAirport = await GetDestinationFor(sourceAirport, item.Value.ToString());

                if (!string.IsNullOrEmpty(destAirport))
                {
                    // If we've got a value add it to the cache
                    DataCache.Instance.AddDestination(sourceAirport, destAirport);
                }
            }

            // Build a list from the existing destinations (once we've requested all of them) and update the search results
            var existingDestinations = DataCache.Instance.GetDestinations(sourceAirport);
            lstAlternativeDestinations.DataSource = existingDestinations;
            lstAlternativeDestinations.DataBind();
        }

        /// <summary>
        /// Asynchronously make the request to the API for a flight from source to destination
        /// </summary>
        /// <param name="sourceAirport">The source airport</param>
        /// <param name="destinationAirport">The destination airport</param>
        /// <returns>The name of the destination airport if flights were found</returns>
        public async static Task<string> GetDestinationFor(string sourceAirport, string destinationAirport)
        {
            IRestClient restClient = new RestClient();

            // Build the API URL
            var fullUrl = string.Format("{0}flights/{1}/{2}/{3}", _baseURL, sourceAirport, destinationAirport, 0);

            // Make the request
            var flightResults = await Task.Run(() => restClient.Get<FlightResults>(fullUrl));

            if (flightResults.flights.Count > 0)
            {
                // If we found flights, it's a valid destination
                return AirportNameTranslation.Instance.GetAirportName(flightResults.flights.FirstOrDefault().outbound.destinationCode);
            }

            // Otherwise, we didn't find any flights so return empty string.
            return string.Empty;
        }
    }
}
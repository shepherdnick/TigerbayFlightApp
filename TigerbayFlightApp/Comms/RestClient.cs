using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace TigerbayFlightApp
{
    public class RestClient : IRestClient
    {
        static JavaScriptSerializer _serializer = new JavaScriptSerializer();
        int _retryCount = 20; // Number of times to retry the API if an Internal Server Error is thrown

        /// <summary>
        /// Creates a version of an HttpWebRequest object to be used when calling the API
        /// </summary>
        /// <param name="url">The endpoint of the request</param>
        /// <returns>The HttpWebRequest object</returns>
        private HttpWebRequest CreateRequest(string url)
        {
            // Create the request object
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Add custom headers
            request.Headers.Add("Authorization", "Bearer 89EF8594E2EA6756BAC84D26D75F8");

            // Content type
            request.ContentType = "application/json";

            return request;
        }

        /// <summary>
        /// Execute the request received.  Deserializes the returned object to the type provided.
        /// </summary>
        /// <typeparam name="T">The type of the returned object to deserialize</typeparam>
        /// <param name="request">The bundled request object to send to the endpoint</param>
        /// <returns>A return object</returns>
        private T Execute<T>(HttpWebRequest request)
        {
            // Try and get a response, if we don't then just process the event
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // In case we need to find the keys <TO REMOVE>
                string result = "Status code: " + (int)response.StatusCode + " " + response.StatusCode + "\r\n";
                foreach (string key in response.Headers.Keys)
                {
                    result += string.Format("{0}: {1} \r\n", key, response.Headers[key]);
                }

                // Read the response data for the object
                string resultObject = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        resultObject += streamReader.ReadToEnd();
                    }
                }

                try
                {
                    // Safely try to deserialize the object <TO COME BACK TO>
                    var returnObject = _serializer.Deserialize<T>(resultObject);
                    return returnObject;
                }
                catch (ArgumentNullException ane)
                {
                    throw ane;
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to deserialize object", ex);
                }
            }
        }

        /// <summary>
        /// Makes a REST GET call to the URL provided.  Deserializes the returned object as the provided return type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the response</typeparam>
        /// <param name="url">The endpoint of the request</param>
        /// <returns>The resulting object if it could be deserialized</returns>
        public T Get<T>(string url)
        {
            var request = CreateRequest(url);
            request.Method = "GET";

            try
            {
                return Execute<T>(request);
            }
            catch (WebException wex)
            {
                // This API throws a lot of Internal Server Errors, but we can just submit the request again and it will usually work.
                if (wex.Response != null && ((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.InternalServerError)
                {
                    _retryCount--;
                    if (_retryCount > 0)
                    {
                        return Get<T>(url);
                    }
                }

                throw wex;
            }
        }

        /// <summary>
        /// Makes a REST POST call to the URL provided.  Serializes the post data object before making the request.
        /// Deserializes the returned object as the provided return type.
        /// </summary>
        /// <typeparam name="TI">The type of the input object to serialize</typeparam>
        /// <typeparam name="TO">The type of the output object to serialize</typeparam>
        /// <param name="url">The endpoint of the request</param>
        /// <param name="data">The data to serialize</param>
        /// <returns>The resulting object if it could be deserialized</returns>
        public TO Post<TI, TO>(string url, TI data)
        {
            var request = CreateRequest(url);
            request.Method = "POST";

            string serializedData = _serializer.Serialize(data);

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    writer.Write(serializedData);
                }
            }

            return Execute<TO>(request);
        }
    }
}
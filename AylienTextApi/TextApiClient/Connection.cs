#region License
/*
Copyright 2016 Aylien, Inc. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aylien.TextApi
{
    class Connection
    {
        static readonly HttpClient client;
        HttpRequestMessage Request;

        static Connection()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(Configuration.BaseUri)
            };

            client.DefaultRequestHeaders.ConnectionClose = false;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Configuration.defaultUserAgent);
        }

        ApiParameters Parameters { get; set; }
        string RequestUri { set; get; }

        public Connection(string endpoint, ApiParameters parameters, Configuration configuration)
        {
            RequestUri = Configuration.BasePath + endpoint;
            Parameters = parameters;
            compileRequestParams(configuration);
        }

        internal async Task<Response> requestAsync()
        {
            try
            {
                var response = await client.SendAsync(Request).ConfigureAwait(false);
                Request.Dispose();
                Request = null;

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Response returnResponse = new Response(responseString, response.Headers);
                response.Content.Dispose();
                response.Dispose();

                return returnResponse;
            }
            catch (WebException we)
            {
                throw new Error(we, true);
            }
        }

        void compileRequestParams(Configuration configuration)
        {
            var url = GetUrl(configuration);
            var _request = new HttpRequestMessage(configuration.Method, url);

            _request.Headers.Add(Configuration.Headers["AppKey"], configuration.AppKey);
            _request.Headers.Add(Configuration.Headers["AppId"], configuration.AppId);

            if (configuration.Method == HttpMethod.Post)
            {
                var postData = Parameters.Aggregate("",
                  (memo, pair) =>
                     "&" + pair.First().Key + "=" + EscapeDataString(pair.First().Value) + memo
                  ).Substring(1);

                var content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
                _request.Content = content;
            }

            Request = _request;
        }

        string GetUrl(Configuration configuration)
        {
            if (configuration.Method == HttpMethod.Get)
            {
                var query = Parameters.Aggregate("",
                  (memo, pair) =>
                     "&" + pair.First().Key + "=" + EscapeDataString(pair.First().Value) + memo
                     );

                if (query != null && query.Length > 2)
                {
                    query = query.Substring(1);
                }

                return RequestUri + (Parameters.Count > 0 ? "?" + query : string.Empty);
            }

            return RequestUri;
        }

        /// <summary>
        /// Improved EscapeDataString to replace Url.EscapeDataString, which has a 64K string limitation.
        /// </summary>
        /// <param name="stringToEscape"></param>
        /// <returns></returns>
        string EscapeDataString(string stringToEscape) {
            const int chunkSize = 32766;                // Ensure that this will work on any .Net platform, incl Store / portable
            var originalSize = stringToEscape.Length;   // Save the original size to make it easier to allocate the StringBuilder at 10% larger to account for encoding.
            var loops = originalSize / chunkSize;       // Number of chunkSize sized loops.

            // Allocate the StringBuilder with some 'air' for encoded characters.
            StringBuilder sb = new StringBuilder(originalSize + originalSize / 10);

            // Iterate the string by chunkSize, and call the native .Net API for each safe-sized chunk.
            for (int loop = 0; loop <= loops; loop++) {
                if (loop < loops) {
                    sb.Append(Uri.EscapeDataString(stringToEscape.Substring(chunkSize * loop, chunkSize)));
                }
                else {
                    sb.Append(Uri.EscapeDataString(stringToEscape.Substring(chunkSize * loop)));
                }
            }

            // Return the chunked and escaped string.
            return sb.ToString();
        }
    }

    class Response
    {
        public Response(string responseString, HttpResponseHeaders headers)
        {
            ResponseResult = responseString;
            ResponseHeader = headers;
        }

        internal string ResponseResult { get; set; }
        internal HttpResponseHeaders ResponseHeader { get; set; }
    }
}

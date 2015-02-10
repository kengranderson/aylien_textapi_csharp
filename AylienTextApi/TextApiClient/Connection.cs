﻿#region License
/*
Copyright 2015 Aylien, Inc. All Rights Reserved.

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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Aylien.TextApi
{
    class Connection
    {
        private List<Dictionary<string, string>> Parameters { get; set; }
        private HttpWebRequest Request { set; get; }
        private string RequestUri { set; get; }

        public Connection(string endpoint, List<Dictionary<string, string>> parameters, Configuration configuration)
        {
            RequestUri = configuration.BaseUri + endpoint;
            Parameters = parameters;
            compileRequestParams(configuration);
        }

        internal Response request()
        {
            try
            {
                var response = (HttpWebResponse)Request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                Response returnResponse = new Response(responseString, response.Headers);

                return returnResponse;
            }
            catch (WebException we)
            {
                throw new Error(we, true);
            }
        }

        private void compileRequestParams(Configuration configuration)
        {
            HttpWebRequest request;
            if (configuration.Method == "POST")
            {
                request = (HttpWebRequest)WebRequest.Create(RequestUri);
                request.Method = configuration.Method;

                var postData = Parameters.Aggregate("",
                  (memo, pair) =>
                     "&" + pair.First().Key + "=" + Uri.EscapeDataString(pair.First().Value) + memo
                  ).Substring(1);

                var data = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            else if (configuration.Method == "GET")
            {
                var query = Parameters.Aggregate("",
                  (memo, pair) =>
                     "&" + pair.First().Key + "=" + Uri.EscapeDataString(pair.First().Value) + memo
                     );

                if (query != null && query.Length > 2)
                    query = query.Substring(1);

                if (Parameters.Count > 0)
                    request = (HttpWebRequest)WebRequest.Create(RequestUri + "?" + query);
                else
                    request = (HttpWebRequest)WebRequest.Create(RequestUri);
                request.Method = configuration.Method;
            }
            else
            {
                throw new ArgumentException("Method should be GET or POST.");
            }

            request.Headers.Add(Configuration.Headers["AppKey"], configuration.AppKey);
            request.Headers.Add(Configuration.Headers["AppId"], configuration.AppId);
            request.UserAgent = configuration.UserAgent;

            Request = request;
        }
    }

    class Response
    {
        public Response(string responseString, WebHeaderCollection headers)
        {
            ResponseResult = responseString;
            ResponseHeader = headers;
        }

        internal string ResponseResult { get; set; }
        internal WebHeaderCollection ResponseHeader { get; set; }
    }
}

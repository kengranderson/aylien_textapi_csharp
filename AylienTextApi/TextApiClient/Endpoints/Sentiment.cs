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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Aylien.TextApi
{
    public class Sentiment : Base
    {
        public Sentiment(Configuration config) : base(config) { }

        internal Response call(string url, string text, string mode)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters["url"] = url;

            if (!String.IsNullOrWhiteSpace(text))
                parameters["text"] = text;

            if (!String.IsNullOrWhiteSpace(mode))
                parameters["mode"] = mode;

            Connection connection = new Connection(Configuration.Endpoints["Sentiment"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        public string Text { get; set; }
        public string Subjectivity { get; set; }

        [JsonProperty("subjectivity_confidence")]
        public double SubjectivityConfidence { get; set; }
        public string Polarity { get; set; }
        [JsonProperty("polarity_confidence")]
        public double PolarityConfidence { get; set; }

        private void populateData(string jsonString)
        {
            Sentiment m = JsonConvert.DeserializeObject<Sentiment>(jsonString);

            Text = m.Text;
            Subjectivity = m.Subjectivity;
            SubjectivityConfidence = m.SubjectivityConfidence;
            Polarity = m.Polarity;
            PolarityConfidence = m.PolarityConfidence;
        }
    }
}

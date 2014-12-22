﻿#region License
/*
Copyright 2014 Aylien, Inc. All Rights Reserved.

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
    public class Summarize : Base
    {
        public Summarize(Configuration config) : base(config) { }

        internal Response call(string text, string title, string url, string mode, string sentencesNumber, string sentencesPercentage)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!String.IsNullOrWhiteSpace(text))
                parameters["text"] = text;

            if (!String.IsNullOrWhiteSpace(title))
                parameters["title"] = title;

            if (!String.IsNullOrWhiteSpace(url))
                parameters["url"] = url;

            if (!String.IsNullOrWhiteSpace(mode))
                parameters["mode"] = mode;

            if (!String.IsNullOrWhiteSpace(sentencesNumber) && sentencesNumber != "0")
                parameters["sentences_number"] = sentencesNumber;

            if (!String.IsNullOrWhiteSpace(sentencesPercentage) && sentencesPercentage != "0")
                parameters["sentences_percentage"] = sentencesPercentage;

            Connection connection = new Connection(Configuration.Endpoints["Summarize"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        public string[] Sentences { get; set; }
        public string Text { get; set; }

        private void populateData(string jsonString)
        {
            Summarize m = JsonConvert.DeserializeObject<Summarize>(jsonString);

            Sentences = m.Sentences;
            Text = m.Text;
        }
    }
}

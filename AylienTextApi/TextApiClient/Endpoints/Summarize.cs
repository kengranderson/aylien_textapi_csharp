﻿#region License
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aylien.TextApi
{
    public class Summarize : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public Summarize() { }

        public Summarize(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string text, string title, string url, string mode, string sentencesNumber, string sentencesPercentage)
        {
            var parameters = new ApiParameters(url, text);
            parameters.Add("title", title).
                Add("mode", mode).
                AddNonPositiveInt("sentences_number", sentencesNumber).
                AddNonPositiveInt("sentences_percentage", sentencesPercentage);

            Connection connection = new Connection(Configuration.Endpoints["Summarize"], parameters, configuration);
            var response = await connection.requestAsync().ConfigureAwait(false);
            populateData(response.ResponseResult);

            return response;
        }

        public string[] Sentences { get; set; }
        public string Text { get; set; }

        void populateData(string jsonString)
        {
            Summarize m = JsonConvert.DeserializeObject<Summarize>(jsonString);

            Sentences = m.Sentences;
            Text = m.Text;
        }
    }
}

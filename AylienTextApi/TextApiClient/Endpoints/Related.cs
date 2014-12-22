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
    public class Related : Base
    {
        public Related(Configuration config) : base(config) { }

        internal Response call(string phrase, string count)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!String.IsNullOrWhiteSpace(phrase))
                parameters["phrase"] = phrase;

            if (!String.IsNullOrWhiteSpace(count) && count != "0")
                parameters["count"] = count;

            Connection connection = new Connection(Configuration.Endpoints["Related"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        [JsonProperty("related")]
        public RelatedPhrase[] RelatedPhrases { get; set; }
        public string Phrase { get; set; }
        

        private void populateData(string jsonString)
        {
            Related m = JsonConvert.DeserializeObject<Related>(jsonString);

            Phrase = m.Phrase;
            RelatedPhrases = m.RelatedPhrases;
        }
    }

    public class RelatedPhrase
    {
        public string Phrase { set; get; }
        public float Distance { set; get; }
    }
}

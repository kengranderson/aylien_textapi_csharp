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
    public class Concepts : Base
    {
        public Concepts(Configuration config) : base(config) { }

        internal Response call(string url, string text, string language)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters["url"] = url;

            if (!String.IsNullOrWhiteSpace(text))
                parameters["text"] = text;

            if (!String.IsNullOrWhiteSpace(language))
                parameters["language"] = language;

            Connection connection = new Connection(Configuration.Endpoints["Concepts"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        [JsonProperty("concepts")]
        public Dictionary<string, Concept> ConceptsMember { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }

        private void populateData(string jsonString)
        {
            Concepts m = JsonConvert.DeserializeObject<Concepts>(jsonString);

            Text = m.Text;
            Language = m.Language;
            ConceptsMember = m.ConceptsMember;
        }
    }

    public class Concept
    {
        public SurfaceForm[] SurfaceForms { set; get; }
        public string[] Types { set; get; }
        public int Support { set; get; }
    }

    public class SurfaceForm
    {
        public float Score { set; get; }
        public string String { set; get; }
        public int Offset { set; get; }
    }
}

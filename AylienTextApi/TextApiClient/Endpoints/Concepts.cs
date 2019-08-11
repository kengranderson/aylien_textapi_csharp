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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aylien.TextApi
{
    public class Concepts : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public Concepts() : base() { }

        public Concepts(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string url, string text, string language)
        {
            var parameters = new ApiParameters(url, text, language);
            Connection connection = new Connection(Configuration.Endpoints["Concepts"], parameters, configuration);
            var response = await connection.requestAsync().ConfigureAwait(false);
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

        public override string ToString() => SurfaceForms?.Length > 0 ? SurfaceForms[0].ToString() : base.ToString();
    }

    public class SurfaceForm
    {
        public float Score { set; get; }
        public string String { set; get; }
        public int Offset { set; get; }

        public override string ToString() => $"{String} [{Score}]";
    }
}

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

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Aylien.TextApi
{
    public class ClassifyByTaxonomy : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public ClassifyByTaxonomy() : base() { }

        public ClassifyByTaxonomy(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string taxonomy, string url, string text, string language)
        {
            try
            {
                Exception = null;

                var parameters = new ApiParameters(url, text, language);

                if (string.IsNullOrEmpty(taxonomy))
                    throw new Error("Invalid taxonomy. Taxonomy can't be blank.");

                var endpoint = Configuration.Endpoints["ClassifyByTaxonomy"].Replace(":taxonomy", taxonomy);
                Connection connection = new Connection(endpoint, parameters, configuration);
                var response = await connection.requestAsync().ConfigureAwait(false);
                callIf(populateData, response.ResponseResult);

                return response;
            }
            catch (Exception ex)
            {
                Exception = ex;
                return null;
            }
        }

        public TaxonomicClassification[] Categories { get; set; }
        public string Text { get; set; }
        public string Taxonomy { get; set; }
        public string Language { get; set; }

        private void populateData(string jsonString)
        {
            ClassifyByTaxonomy m = JsonConvert.DeserializeObject<ClassifyByTaxonomy>(jsonString);

            Categories = m?.Categories;
            Text = m?.Text;
            Language = m?.Language;
            Taxonomy = m?.Taxonomy;
        }
    }

    public class TaxonomicClassification
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public float Score { get; set; }
        public bool Confident { get; set; }
        public Link[] Links { get; set; }

        public override string ToString() => $"{Label} [{Score}]";
    }

    public class Link
    {
        public string Rel { get; set; }
        
        [JsonProperty("link")]
        public string Url { get; set; }

        public override string ToString() => $"{Rel}: {Url}";
    }
}

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
    public class Entities : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public Entities() : base() { }

        public Entities(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string url, string text)
        {
            try
            {
                Exception = null;

                var parameters = new ApiParameters(url, text);
                Connection connection = new Connection(Configuration.Endpoints["Entities"], parameters, configuration);
                var response = await connection.requestAsync().ConfigureAwait(false);
                populateData(response.ResponseResult);

                return response;
            }
            catch (Exception ex)
            {
                Exception = ex;
                return null;
            }
        }

        [JsonProperty("entities")]
        public Entity EntitiesMember { get; set; }
        public string Text { get; set; }

        private void populateData(string jsonString)
        {
            Entities m = JsonConvert.DeserializeObject<Entities>(jsonString);

            Text = m?.Text;
            EntitiesMember = m?.EntitiesMember;
        }
    }

    public class Entity
    {
        public string[] Organization {get; set;}
        public string[] Location {get; set;}
        public string[] Person {get; set;}
        public string[] Keyword {get; set;}
        public string[] Date {get; set;}
        public string[] Money { get; set; }
        public string[] Percentage { get; set; }
        public string[] Time {get; set;}
        public string[] Url { get; set; }
        public string[] Email { get; set; }
        public string[] Phone { get; set; }
    }
}

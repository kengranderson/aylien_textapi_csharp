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
    public class Language : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public Language() : base() { }

        public Language(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string url, string text)
        {
            try
            {
                Exception = null;

                var parameters = new ApiParameters(url, text);
                Connection connection = new Connection(Configuration.Endpoints["Language"], parameters, configuration);
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

    public string Lang { get; set; }
        public string Text { get; set; }
        public float? Confidence { get; set; }

        private void populateData(string jsonString)
        {
            Language m = JsonConvert.DeserializeObject<Language>(jsonString);

            Text = m?.Text;
            Lang = m?.Lang;
            Confidence = m?.Confidence;
        }
    }
}

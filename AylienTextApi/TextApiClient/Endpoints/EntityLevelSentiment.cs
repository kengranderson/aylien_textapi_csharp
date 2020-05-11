#region License
/*
Copyright 2018 Aylien, Inc. All Rights Reserved.

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
    public class EntityLevelSentiment : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public EntityLevelSentiment() : base() { }

        public EntityLevelSentiment(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string url, string text)
        {
            try
            {
                Exception = null;


                var parameters = new ApiParameters(url, text);
                Connection connection = new Connection(Configuration.Endpoints["Elsa"], parameters, configuration);
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

        public string Text { get; set; }
        public EntityLevelSentimentEntity[] Entities { get; set; }

        private void populateData(string jsonString)
        {
            EntityLevelSentiment m = JsonConvert.DeserializeObject<EntityLevelSentiment>(jsonString);

            Text = m?.Text;
            Entities = m?.Entities;
        }
    }

    public class EntityLevelSentimentEntity
    {
        public EntityLevelSentimentMention[] Mentions { set; get; }
        [JsonProperty("overall_sentiment")]
        public EntityLevelSentimentOverallSentiment OverallSentiment { set; get; }
        public EntityLevelSentimentLink[] Links { set; get; }
        public string Type { get; set; }
    }

    public class EntityLevelSentimentOverallSentiment  {
        public string Polarity { get; set; }
        public float Confidence  { get; set; }
    }

    public class EntityLevelSentimentLink {
        public string URI { set; get; }
        public string Provider { set; get; }
        public string[] Types { set; get; } 
        public float Confidence { set; get; }
    }

    public class EntityLevelSentimentMention {
	public int Offset { set; get; }
	public float Confidence { set; get; }
	public string Text { set; get; }
	public EntityLevelSentimentOverallSentiment Sentiment { set; get; }
    }
}

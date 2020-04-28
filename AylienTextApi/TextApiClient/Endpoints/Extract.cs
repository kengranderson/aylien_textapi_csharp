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
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;

namespace Aylien.TextApi
{
    public class Extract : Base
    {
        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        public Extract() { }

        public Extract(Configuration config) : base(config) { }

        internal async Task<Response> callAsync(string url, string html, string bestImage, string keepHtmlFormatting)
        {
            try
            {
                Exception = null;

                var parameters = new ApiParameters(url).
                    Add("html", html).
                    Add("best_image", bestImage).
                    Add("keep_html_formatting", keepHtmlFormatting);

                Connection connection = new Connection(Configuration.Endpoints["Extract"], parameters, configuration);
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

        public string Title { get; set; }
        public string Article { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public DateTime? PublishDate { get; set; }
        public string[] Videos { get; set; }
        public string[] Feeds { get; set; }

        void populateData(string jsonString)
        {
            Extract m = JsonConvert.DeserializeObject<Extract>(jsonString,
                new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" });

            Title = m.Title;
            Article = m.Article;
            Image = m.Image;
            Author = m.Author;
            Feeds = m.Feeds;
            Videos = m.Videos;
            PublishDate = m.PublishDate;
        }
    }
}

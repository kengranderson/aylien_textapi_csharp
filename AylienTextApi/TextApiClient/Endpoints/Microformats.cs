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

namespace Aylien.TextApi
{
    public class Microformats : Base
    {
        public Microformats(Configuration config) : base(config) { }

        internal Response call(string url)
        {
            List<Dictionary<string, string>> parameters = new List<Dictionary<string, string>>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters.Add(new Dictionary<string, string> { {"url", url} });

            Connection connection = new Connection(Configuration.Endpoints["Microformats"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        [JsonProperty("hCards")]
        public HCard[] HCards { get; set; }

        private void populateData(string jsonString)
        {
            Microformats m = JsonConvert.DeserializeObject<Microformats>(jsonString);
            HCards = m.HCards;
        }
    }

    public class HCard
    {
        public string Birthday { get; set; }
        public string Organization { get; set; }
        public string TelephoneNumber { get; set; }
        public Location Location { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string FullName { get; set; }
        public StructuredName StructuredName { get; set; }
        public string Logo { get; set; }
        public string Id { get; set; }
        public string Note { get; set; }
        public Address Address { get; set; }
        public string Category { get; set; }
    }

    public class Location
    {
        public string Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class StructuredName
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string HonorificSuffix { get; set; }
        public string Id { get; set; }
        public string AdditionalName { get; set; }
        public string honorificPrefix { get; set; }
    }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public string Id { get; set; }
        public string Region { get; set; }
        public string Locality { get; set; }
    }
}

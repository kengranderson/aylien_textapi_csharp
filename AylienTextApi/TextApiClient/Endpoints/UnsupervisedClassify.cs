﻿#region License
/*
Copyright 2015 Aylien, Inc. All Rights Reserved.

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
    public class UnsupervisedClassify : Base
    {
        public UnsupervisedClassify(Configuration config) : base(config) { }

        internal Response call(string url, string text, string[] classes, string numberOfConcepts)
        {
            List<Dictionary<string, string>> parameters = new List<Dictionary<string, string>>();

            if (!String.IsNullOrWhiteSpace(url))
                parameters.Add(new Dictionary<string, string> { { "url", url } });

            if (!String.IsNullOrWhiteSpace(text))
                parameters.Add(new Dictionary<string, string> { { "text", text } });

            if (!String.IsNullOrWhiteSpace(numberOfConcepts))
                parameters.Add(new Dictionary<string, string> { { "number_of_concepts", numberOfConcepts } });

            foreach (string klass in classes)
            {
                parameters.Add(new Dictionary<string, string> { { "class", klass } });
            }
            
            Connection connection = new Connection(Configuration.Endpoints["UnsupervisedClassify"], parameters, configuration);
            var response = connection.request();
            populateData(response.ResponseResult);

            return response;
        }

        public ClassificationClass[] Classes { get; set; }
        public string Text { get; set; }

        private void populateData(string jsonString)
        {
            UnsupervisedClassify m = JsonConvert.DeserializeObject<UnsupervisedClassify>(jsonString);

            Classes = m.Classes;
            Text = m.Text;
        }
    }

    public class ClassificationClass
    {
        public string Label { get; set; }
        public float Score { get; set; }
    }
}

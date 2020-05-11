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


using System;

namespace Aylien.TextApi
{
    public class Base
    {
        protected Configuration configuration;

        /// <summary>
        /// Default constructor added to provide better serialization support.
        /// </summary>
        protected Base() {
        }

        public Base(Configuration config)
        {
            configuration = config;
        }

        public Exception Exception { get; set; }

        protected void callIf(Action<string> method, string parm)
        { 
            if (parm != null)
            {
                method(parm);
            }
        }

        //protected void AddUrl(List<Dictionary<string, string>> dict, string key, string value)
        //{
        //    if (!string.IsNullOrWhiteSpace(value))
        //        dict.Add(new Dictionary<string, string> { { key, value } });
        //}
    }
}

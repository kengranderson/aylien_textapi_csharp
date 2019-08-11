using System.Collections.Generic;

namespace Aylien.TextApi
{
    internal class ApiParameters : List<Dictionary<string, string>>
    {
        public ApiParameters()
        {
        }

        public ApiParameters(string url)
        {
            Add("url", url);
        }

        public ApiParameters(string url, string text) : this(url)
        {
            Add("text", text);
        }

        public ApiParameters(string url, string text, string language) : this(url, text)
        {
            Add("language", language);
        }

        public ApiParameters Add(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                Add(new Dictionary<string, string> { { key, value } });

            return this;
        }

        public ApiParameters AddNonPositiveInt(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && int.Parse(value) > 0)
                Add(new Dictionary<string, string> { { key, value } });

            return this;
        }
    }
}

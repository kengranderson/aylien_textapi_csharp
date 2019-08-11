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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aylien.TextApi
{
    /// <summary>
    /// A Client can make calls to the Text API.
    /// </summary>
    public class Client
    {
        readonly Configuration configuration;
        Dictionary<string, int> rateLimit = new Dictionary<string, int>
        {
            {"Limit", -1},
            { "Remaining", -1},
            { "Reset", -1}
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="appId">A key which you can get it from Aylien developer website</param>
        /// <param name="appKey">A key which you can get it from Aylien developer website</param>
        public Client(string appId, string appKey)
        {
            configuration = new Configuration(appId, appKey);
        }

        /// <summary>
        /// Extracts the main body of article, including embedded media such as
        /// images &amp; videos from a URL and removes all the surrounding clutter.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="html">HTML as string</param>
        /// <param name="bestImage">Whether extract the best image of the article</param>
        /// <returns>A <see cref="ExtractAsync"/></returns>
        public async Task<Extract> ExtractAsync(string url = null, string html = null, bool bestImage = false, bool keepHtmlFormatting = false)
        {
            var extract = new Extract(configuration);
            var r = await extract.callAsync(url, html, bestImage.ToString(), keepHtmlFormatting.ToString()).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return extract;
        }

        /// <summary>
        /// Summarizes an article into a few key sentences.
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="title">Title</param>
        /// <param name="url">A Valid URL</param>
        /// <param name="mode">Analyze mode. Valid options are default
        ///   and short. Default is default. short mode produces shorter sentences.</param>
        /// <param name="sentencesNumber">Number of sentences to be returned.
        ///   Only in default mode (not applicable to short mode).
        ///   Default value is 5.
        ///   has precedence over sentencesPercentage.</param>
        /// <param name="sentencesPercentage">Percentage of sentences to be returned.
        ///   Only in default mode (not applicable to short mode).
        ///   Possible range is 1-100.
        ///   sentencesNumber has precedence over this parameter.</param>
        /// <returns>A <see cref="SummarizeAsync"/></returns>
        public async Task<Summarize> SummarizeAsync(string text = null, string title = null, string url = null, string mode = null, int sentencesNumber = 0, int sentencesPercentage = 0)
        {
            var summarize = new Summarize(configuration);
            var r = await summarize.callAsync(text, title, url, mode, sentencesNumber.ToString(), sentencesPercentage.ToString()).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return summarize;
        }

        /// <summary>
        /// Classifies a piece of text according to IPTC NewsCode standard.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        ///  en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        ///  detect the language. Default is en.</param>
        /// <returns>A <see cref="ClassifyAsync"/></returns>
        public async Task<Classify> ClassifyAsync(string url = null, string text = null, string language = null)
        {
            var classify = new Classify(configuration);
            var r = await classify.callAsync(url, text, language).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return classify;
        }

        /// <summary>
        /// Classifies a piece of text according to specified taxonomy.
        /// </summary>
        /// <param name="taxonomy">Taxonomy to classify according to</param>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        ///  en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        ///  detect the language. Default is en.</param>
        /// <returns>A <see cref="ClassifyAsync"/></returns>
        public async Task<ClassifyByTaxonomy> ClassifyByTaxonomyAsync(string taxonomy, string url = null, string text = null, string language = null)
        {
            var classify = new ClassifyByTaxonomy(configuration);
            var r = await classify.callAsync(taxonomy, url, text, language).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return classify;
        }

        /// <summary>
        /// Detects sentiment of a document in terms of
        /// polarity ("positive" or "negative") and
        /// subjectivity ("subjective" or "objective").
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="mode">Analyze mode. Valid options are
        ///  tweet, and document. Default is tweet.</param>
        ///  <param name="language">Language</param>
        /// <returns>A <see cref="SentimentAsync"/></returns>
        public async Task<Sentiment> SentimentAsync(string url = null, string text = null, string mode = null, string language = null)
        {
            var sentiment = new Sentiment(configuration);
            var r = await sentiment.callAsync(url, text, mode, language).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return sentiment;
        }

        /// <summary>
        /// Extracts named entities (people, organizations and locations) and
        /// values (URLs, emails, telephone numbers, currency amounts and percentages)
        /// mentioned in a body of text.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="EntitiesAsync"/></returns>
        public async Task<Entities> EntitiesAsync(string url = null, string text = null)
        {
            var entities = new Entities(configuration);
            var r = await entities.callAsync(url, text).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return entities;
        }

        /// <summary>
        /// Extracts named entities mentioned in a document, disambiguates and
        /// cross-links them to DBPedia and Linked Data entities, along with
        /// their semantic types (including DBPedia and schema.org).
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        /// en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        /// detect the language. Default is en.</param>
        /// <returns>A <see cref="ConceptsAsync"/></returns>
        public async Task<Concepts> ConceptsAsync(string url = null, string text = null, string language = null)
        {
            var concepts = new Concepts(configuration);
            var r = await concepts.callAsync(url, text, language).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return concepts;
        }

        /// <summary>
        /// Suggests hashtags describing the document.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="language">Language of text. Valid options are
        ///  en, de, fr, es, it, pt, and auto. If set to auto, it'll try to
        ///  detect the language. Default is en.</param>
        /// <returns>A <see cref="HashtagsAsync"/></returns>
        public async Task<Hashtags> HashtagsAsync(string url = null, string text = null, string language = null)
        {
            var hashtags = new Hashtags(configuration);
            var r = await hashtags.callAsync(url, text, language).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return hashtags;
        }

        /// <summary>
        /// Detects the main language a document is written in and returns it
        /// in ISO 639-1 format.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="LanguageAsync"/></returns>
        public async Task<Language> LanguageAsync(string url = null, string text = null)
        {
            var language = new Language(configuration);
            var r = await language.callAsync(url, text).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return language;
        }

        /// <summary>
        /// Runs multiple analysis operations in one API call.
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <param name="endpoints">List of operations</param>
        /// <returns>A <see cref="CombinedAsync"/></returns>
        public async Task<Combined> CombinedAsync(string url = null, string text = null, string[] endpoints = null)
        {
            var combined = new Combined(configuration);
            var r = await combined.callAsync(url, text, endpoints).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return combined;
        }

        /// <summary>
        /// Assigns relevant tags to an image.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>A <see cref="ImageTagsAsync"/></returns>
        public async Task<ImageTags> ImageTagsAsync(string url)
        {
            var imageTags = new ImageTags(configuration);
            var r = await imageTags.callAsync(url).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return imageTags;
        }

        /// <summary>
        /// Detects aspects and sentiment of a body of text.
        /// </summary>
        /// <param name="domain">Domain which document belongs to</param>
        /// <param name="url">URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="AspectBasedSentimentAsync"/></returns>
        public async Task<AspectBasedSentiment> AspectBasedSentimentAsync(string domain, string url = null, string text = null)
        {
            var aspectBasedSentiment = new AspectBasedSentiment(configuration);
            var r = await aspectBasedSentiment.callAsync(domain, url, text).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return aspectBasedSentiment;
        }

        /// <summary>
        /// Returns the sentiment towards entities on given text
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <param name="text">Text</param>
        /// <returns>A <see cref="EntityLevelSentimentAsync"/></returns>
        public async Task<EntityLevelSentiment> EntityLevelSentimentAsync(string url = null, string text = null)
        {
            var elsa = new EntityLevelSentiment(configuration);
            var r = await elsa.callAsync(url, text).ConfigureAwait(false);
            extractRateLimitParameters(r);
            return elsa;
        }

        /// <summary>
        /// Returns Rate Limit of API calls.
        /// </summary>
        /// <returns> Return a Dictionary&lt;string, int&gt; of rate limit</returns>
        public Dictionary<string, int> RateLimit
        {
            get
            {
                if (rateLimit["Limit"] == -1 && rateLimit["Remaining"] == -1 && rateLimit["Reset"] == -1)
                {
                    var lang = new Language(configuration);
                    var r = Task.Run(() => lang.callAsync(null, "Test")).Result;
                    extractRateLimitParameters(r);
                }
                return rateLimit;
            }
            private set
            {
                rateLimit = value;
            }
        }

        void extractRateLimitParameters(Response r) {
            RateLimit = new Dictionary<string, int>
            {
                {"Limit", SafeParse(r, "X-RateLimit-Limit", -1)},
                {"Remaining", SafeParse(r, "X-RateLimit-Remaining", -1)},
                {"Reset", SafeParse(r, "X-RateLimit-Reset", -1)}
            };
        }

        /// <summary>
        /// Added this method because extractRateLimitParameters throws null exceptions 
        /// when the API response does not include X-RateLimit headers.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int SafeParse(Response r, string key, int defaultValue) {
            int.TryParse(r.ResponseHeader.GetValues(key).FirstOrDefault(), out defaultValue);
            return defaultValue;
        }
    }
}

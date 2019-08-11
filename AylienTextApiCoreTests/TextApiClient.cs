//#define PAID_PLAN

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Aylien.TextApi.Tests
{
    [TestClass]
    public class UnitTestClient
    {
        private string url, text, phrase, title, imageUrl, taxonomy, domain;
        private Client client;

        private void setRequireVariables()
        {
            var appId = "04e81357";
            var appKey = "f005be74d17ca53378516ce317f14f9a";

            client = new Client(appId, appKey);
            url = "http://www.bbc.co.uk/news/world-australia-30544493#sa-ns_mchannel=rss&ns_source=PublicRSS20-sa";
            imageUrl = "https://www.fujifilmusa.com/products/digital_cameras/x/fujifilm_x_m1/sample_images/img/index/ff_x_m1_018.JPG";
            text = "John is a very good football player!";
            phrase = "Dublin";
            title = "Test title";
            taxonomy = "iab-qag";
            domain = "airlines";
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void ShouldThrowErrorWithUnauthenticatedClient()
        {
            Client invalidClient = new Client("WrongAppId", "WrongAppKey");
            var task = Task.Run(() => invalidClient.SentimentAsync(text: "John is a bad football player"));
            var sentiment = task.Result;
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassify()
        {
            setRequireVariables();
            var classify = Task.Run(() => client.ClassifyAsync(text: text)).Result;

            Assert.IsInstanceOfType(classify, typeof(Classify));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfConcepts()
        {
            setRequireVariables();
            var concepts = Task.Run(() => client.ConceptsAsync(text: text)).Result;

            Assert.IsInstanceOfType(concepts, typeof(Concepts));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfEntities()
        {
            setRequireVariables();
            var entities = Task.Run(() => client.EntitiesAsync(text: text)).Result;

            Assert.IsInstanceOfType(entities, typeof(Entities));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfExtract()
        {
            setRequireVariables();
            var extract = Task.Run(() => client.ExtractAsync(url: url)).Result;

            Assert.IsInstanceOfType(extract, typeof(Extract));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfHashtags()
        {
            setRequireVariables();
            var hashtags = Task.Run(() => client.HashtagsAsync(text: text)).Result;

            Assert.IsInstanceOfType(hashtags, typeof(Hashtags));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfLanguage()
        {
            setRequireVariables();
            var language = Task.Run(() => client.LanguageAsync(text: text)).Result;

            Assert.IsInstanceOfType(language, typeof(Language));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSentiment()
        {
            setRequireVariables();
            var sentiment = Task.Run(() => client.SentimentAsync(text: text)).Result;

            Assert.IsInstanceOfType(sentiment, typeof(Sentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSummarize()
        {
            setRequireVariables();
            var summarize = Task.Run(() => client.SummarizeAsync(text: text, title: title)).Result;

            Assert.IsInstanceOfType(summarize, typeof(Summarize));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfCombined()
        {
            setRequireVariables();
            string[] endpoints = new string[] { "classify", "concepts", "entities", "extract", "hashtags", "language", "sentiment", "summarize" };
            var combined = Task.Run(() => client.CombinedAsync(url: url, endpoints: endpoints)).Result;

            Assert.IsInstanceOfType(combined, typeof(Combined));
        }

#if PAID_PLAN

        [TestMethod]
        public void ShouldReturnAnInstanceOfImageTags()
        {
            setRequireVariables();
            var imageTags = Task.Run(() => client.ImageTagsAsync(url: imageUrl)).Result;

            Assert.IsInstanceOfType(imageTags, typeof(ImageTags));
        }

#endif

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassifyByTaxonomy()
        {
            setRequireVariables();
            var classifyByTaxonomy = Task.Run(() => client.ClassifyByTaxonomyAsync(taxonomy, url: url)).Result;

            Assert.IsInstanceOfType(classifyByTaxonomy, typeof(ClassifyByTaxonomy));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfAspectBasedSentiment()
        {
            setRequireVariables();
            var aspectBasedSentiment = Task.Run(() => client.AspectBasedSentimentAsync(domain, text: text)).Result;

            Assert.IsInstanceOfType(aspectBasedSentiment, typeof(AspectBasedSentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfElsa()
        {
            setRequireVariables();
            var elsa = Task.Run(() => client.EntityLevelSentimentAsync(text: text)).Result;

            Assert.IsInstanceOfType(elsa, typeof(EntityLevelSentiment));
        }

        [TestMethod]
        public void ShouldNotReturnEmptyRateLimit()
        {
            setRequireVariables();
            Dictionary<string, int> rateLimit = client.RateLimit;

            Assert.AreNotEqual(rateLimit["Limit"], -1);
            Assert.AreNotEqual(rateLimit["Remaining"], -1);
            Assert.AreNotEqual(rateLimit["Reset"], -1);
        }
    }
}

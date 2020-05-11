using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Aylien.TextApi.Tests
{
    [TestClass]
    public class UnitTestClient
    {
        private string url, text, title, imageUrl, taxonomy, domain, html;
        private Client client;

        private void setRequireVariables()
        {
            var appId = ConfigurationManager.AppSettings["appId"];
            var appKey = ConfigurationManager.AppSettings["appKey"];

            client = new Client(appId, appKey);
            url = "https://www.wbap.com/2020/05/10/murder-charges-laid-2-suspects-arrested-in-the-georgia-shooting-of-an-african-american-man/";
            imageUrl = "https://consumeraffairs.global.ssl.fastly.net/files/news/hamburger.png";
            text = "John is a very good football player!";
            title = "Test title";
            taxonomy = "iab-qag";
            domain = "airlines";
            html = File.ReadAllText("sample.html");
        }

        [TestMethod]
        [ExpectedException(typeof(Error))]
        public void ShouldThrowErrorWithUnauthenticatedClient()
        {
            Client invalidClient = new Client("WrongAppId", "WrongAppKey");
            Sentiment sentiment = Task.Run(async () => await invalidClient.SentimentAsync(text: "John is a bad football player").ConfigureAwait(false)).Result;
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassify()
        {
            setRequireVariables();
            Classify classify = Task.Run(async () => await client.ClassifyAsync(text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(classify, typeof(Classify));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfConcepts()
        {
            setRequireVariables();
            Concepts concepts = Task.Run(async () => await client.ConceptsAsync(text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(concepts, typeof(Concepts));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfEntities()
        {
            setRequireVariables();
            Entities entities = Task.Run(async () => await client.EntitiesAsync(text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(entities, typeof(Entities));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfExtract()
        {
            setRequireVariables();
            Extract extract = Task.Run(async () => await client.ExtractAsync(url: url).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(extract, typeof(Extract));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfExtractFromHtml()
        {
            setRequireVariables();
            Extract extract = Task.Run(async () => await client.ExtractAsync(html: html).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(extract, typeof(Extract));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfHashtags()
        {
            setRequireVariables();
            Hashtags hashtags = Task.Run(async () => await client.HashtagsAsync(text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(hashtags, typeof(Hashtags));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfLanguage()
        {
            setRequireVariables();
            Language language = Task.Run(async () => await client.LanguageAsync(text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(language, typeof(Language));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSentiment()
        {
            setRequireVariables();
            Sentiment sentiment = Task.Run(async () => await client.SentimentAsync(text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(sentiment, typeof(Sentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSummarize()
        {
            setRequireVariables();
            Summarize summarize = Task.Run(async () => await client.SummarizeAsync(text: text, title: title).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(summarize, typeof(Summarize));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfCombined()
        {
            setRequireVariables();
            string[] endpoints = new string[] { "classify", "concepts", "entities", "extract", "hashtags", "language", "sentiment", "summarize" };
            Combined combined = Task.Run(async () => await client.CombinedAsync(url: url, endpoints: endpoints).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(combined, typeof(Combined));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfImageTags()
        {
            setRequireVariables();
            ImageTags imageTags = Task.Run(async () => await client.ImageTagsAsync(url: imageUrl).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(imageTags, typeof(ImageTags));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassifyByTaxonomy()
        {
            setRequireVariables();
            ClassifyByTaxonomy classifyByTaxonomy = Task.Run(async () => await client.ClassifyByTaxonomyAsync(taxonomy, url: url).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(classifyByTaxonomy, typeof(ClassifyByTaxonomy));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfAspectBasedSentiment()
        {
            setRequireVariables();
            AspectBasedSentiment aspectBasedSentiment = Task.Run(async () => await client.AspectBasedSentimentAsync(domain, text: text).ConfigureAwait(false)).Result;

            Assert.IsInstanceOfType(aspectBasedSentiment, typeof(AspectBasedSentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfElsa()
        {
            setRequireVariables();
            EntityLevelSentiment elsa = Task.Run(async () => await client.EntityLevelSentimentAsync(text: text).ConfigureAwait(false)).Result;

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

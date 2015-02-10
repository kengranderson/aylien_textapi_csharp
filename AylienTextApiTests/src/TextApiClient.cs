using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;

namespace Aylien.TextApi.Tests
{
    [TestClass]
    public class UnitTestClient
    {
        private string url, text, phrase, title;
        private Client client;

        private void setRequireVariables()
        {
            var appId = ConfigurationManager.AppSettings["appId"];
            var appKey = ConfigurationManager.AppSettings["appKey"];

            client = new Client(appId, appKey);
            url = "http://www.bbc.co.uk/news/world-australia-30544493#sa-ns_mchannel=rss&ns_source=PublicRSS20-sa";
            text = "John is a very good football player!";
            phrase = "Dublin";
            title = "Test title";
        }

        [TestMethod]
        [ExpectedException(typeof(Error))]
        public void ShouldThrowErrorWithUnauthenticatedClient()
        {
            Client invalidClient = new Client("WrongAppId", "WrongAppKey");
            Sentiment sentiment = invalidClient.Sentiment(text: text);
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfClassify()
        {
            setRequireVariables();
            Classify classify = client.Classify(text: text);

            Assert.IsInstanceOfType(classify, typeof(Classify));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfConcepts()
        {
            setRequireVariables();
            Concepts concepts = client.Concepts(text: text);

            Assert.IsInstanceOfType(concepts, typeof(Concepts));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfEntities()
        {
            setRequireVariables();
            Entities entities = client.Entities(text: text);

            Assert.IsInstanceOfType(entities, typeof(Entities));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfExtract()
        {
            setRequireVariables();
            Extract extract = client.Extract(url: url);

            Assert.IsInstanceOfType(extract, typeof(Extract));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfHashtags()
        {
            setRequireVariables();
            Hashtags hashtags = client.Hashtags(text: text);

            Assert.IsInstanceOfType(hashtags, typeof(Hashtags));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfLanguage()
        {
            setRequireVariables();
            Language language = client.Language(text: text);

            Assert.IsInstanceOfType(language, typeof(Language));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfRelated()
        {
            setRequireVariables();
            Related related = client.Related(phrase: phrase);

            Assert.IsInstanceOfType(related, typeof(Related));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSentiment()
        {
            setRequireVariables();
            Sentiment sentiment = client.Sentiment(text: text);

            Assert.IsInstanceOfType(sentiment, typeof(Sentiment));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfSummarize()
        {
            setRequireVariables();
            Summarize summarize = client.Summarize(text: text, title: title);

            Assert.IsInstanceOfType(summarize, typeof(Summarize));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfMicroformats()
        {
            setRequireVariables();
            Microformats microformats = client.Microformats(url: url);

            Assert.IsInstanceOfType(microformats, typeof(Microformats));
        }

        [TestMethod]
        public void ShouldReturnAnInstanceOfUnsupervisedClassify()
        {
            setRequireVariables();
            string[] classes = new string[] {"painting", "dancing"};
            UnsupervisedClassify unsupervisedClassify = client.UnsupervisedClassify(url: url, classes: classes);

            Assert.IsInstanceOfType(unsupervisedClassify, typeof(UnsupervisedClassify));
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

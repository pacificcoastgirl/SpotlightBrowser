using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpotlightBrowser;
using System.Linq;
using System.Threading.Tasks;

namespace SpotlightBrowserTester
{
    [TestClass]
    public class SpotlightViewModelTests
    {
        [TestMethod]
        public async Task SpotlightViewModelHasItemsTest()
        {
            // arrange
            SpotlightViewModel systemUnderTest = new SpotlightViewModel();

            // act
            var items = await systemUnderTest.GetItemsAsync();
            
            // assert
            Assert.IsNotNull(items);
            Assert.AreEqual(6, items.ToList().Count);
        }

        [TestMethod]
        public void SpotlightViewModelIsOnlineTest()
        {
            // arrange
            SpotlightViewModel systemUnderTest = new SpotlightViewModel();

            // act
            var isOffline = systemUnderTest.IsOffline;

            // assert
            Assert.IsFalse(isOffline);
        }

        [TestMethod]
        public void SpotlightViewModelIsFeedLoadedTest()
        {
            // arrange
            SpotlightViewModel systemUnderTest = new SpotlightViewModel();

            // act
            var isLoaded = systemUnderTest.IsFeedLoaded;

            // assert
            Assert.IsTrue(isLoaded);
        }

        [TestMethod]
        public void SpotlightViewModelIsFeedErroredTest()
        {
            // arrange
            SpotlightViewModel systemUnderTest = new SpotlightViewModel();

            // act
            var isErrored = systemUnderTest.IsFeedErrored;

            // assert
            Assert.IsFalse(isErrored);
        }
    }

    [TestClass]
    public class FeedReaderTests
    {
        [TestMethod]
        public void CheckIsFeedAvailableTest()
        {
            // arrange
            string url = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
            SpotlightFeedReader systemUnderTest = new SpotlightFeedReader(url);

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;

            // assert
            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public async Task CheckFeedHasItemsTest()
        {
            // arrange
            string url = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
            SpotlightFeedReader systemUnderTest = new SpotlightFeedReader(url);

            // act
            var items = await systemUnderTest.GetFeedAsync();

            // assert
            Assert.IsNotNull(items);
        }

        [TestMethod]
        public async Task CheckFeedHasSixItemsTest()
        {
            // arrange
            string url = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
            SpotlightFeedReader systemUnderTest = new SpotlightFeedReader(url);

            // act
            var root = await systemUnderTest.GetFeedAsync();
            var numItems = root.Items.Count;

            // assert
            Assert.IsNotNull(root);
            Assert.AreEqual(6, numItems);
        }

        [TestMethod]
        public void CheckFeedHasUrlTest()
        {
            // arrange
            string url = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
            SpotlightFeedReader systemUnderTest = new SpotlightFeedReader(url);

            // act
            var urlFromFeedReader = systemUnderTest.Url;

            // assert
            Assert.IsNotNull(urlFromFeedReader);
            Assert.AreEqual(url, urlFromFeedReader);
        }
    }
}

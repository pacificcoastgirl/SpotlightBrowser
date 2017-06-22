using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpotlightBrowser;
using System.Threading.Tasks;

namespace SpotlightBrowserTester
{
    [TestClass]
    public class SpotlightViewModelTests
    {
        [TestMethod]
        public void SpotlightViewModelHasItemsTest()
        {
            // arrange
            SpotlightViewModel systemUnderTest = new SpotlightViewModel();

            // act
            var items = systemUnderTest.Items;

            // assert
            Assert.IsNotNull(items);
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
    }

    [TestClass]
    public class FeedReaderTests
    {
        [TestMethod]
        public void CheckIsFeedAvailableTest()
        {
            // arrange
            string url = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
            FeedReader systemUnderTest = new FeedReader(url);

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
            FeedReader systemUnderTest = new FeedReader(url);

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
            FeedReader systemUnderTest = new FeedReader(url);

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
            FeedReader systemUnderTest = new FeedReader(url);

            // act
            var urlFromFeedReader = systemUnderTest.Url;

            // assert
            Assert.IsNotNull(urlFromFeedReader);
            Assert.AreEqual(url, urlFromFeedReader);
        }
    }
}

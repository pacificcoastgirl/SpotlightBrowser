using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpotlightBrowser;
using System.Linq;
using System.Threading.Tasks;

namespace SpotlightBrowserTester
{
    public class SpotlightBrowserTestOps
    {
        public static string k_validFeedUrl = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
        public static string k_invalidFeedUrl = "";
    }

    [TestClass]
    public class SpotlightViewModelTests
    {
        [TestMethod]
        public async Task SpotlightViewModelHasItemsTest()
        {
            // arrange
            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var items = systemUnderTest.Items;
            var hintText = systemUnderTest.HintText;

            // assert
            Assert.IsNotNull(items);
            Assert.AreEqual(6, items.ToList().Count);
            Assert.AreEqual(string.Empty, hintText);
        }
        
        [TestMethod]
        public async Task SpotlightViewModelIsFeedAvailableTest()
        {
            // arrange
            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;
            var isErrored = systemUnderTest.IsFeedErrored;
            var hintText = systemUnderTest.HintText;

            // assert
            Assert.IsTrue(isAvailable);
            Assert.IsFalse(isErrored);
            Assert.AreEqual(string.Empty, hintText);
        }

        [TestMethod]
        public async Task SpotlightViewModelIsInvalidFeedUnavailableTest()
        {
            // arrange
            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel(SpotlightBrowserTestOps.k_invalidFeedUrl);

            // act
            Assert.IsNotNull(systemUnderTest);
            var isAvailable = systemUnderTest.IsFeedAvailable;
            var isErrored = systemUnderTest.IsFeedErrored;
            var hintText = systemUnderTest.HintText;

            // assert
            Assert.IsFalse(isAvailable);
            Assert.IsTrue(isErrored);
            Assert.AreNotEqual(string.Empty, hintText);
        }

        [TestMethod]
        public async Task SpotlightViewModelRetryTest()
        {
            // arrange
            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel(SpotlightBrowserTestOps.k_invalidFeedUrl);

            // act
            var isAvailableBefore = systemUnderTest.IsFeedAvailable;
            systemUnderTest.Url = SpotlightBrowserTestOps.k_validFeedUrl;
            var retry = systemUnderTest.RetryCommand;
            await retry.ExecuteAsync(null);
            var isAvailableAfter = systemUnderTest.IsFeedAvailable;

            // assert
            Assert.IsFalse(isAvailableBefore);
            Assert.IsTrue(isAvailableAfter);
        }
    }

    [TestClass]
    public class FeedReaderTests
    {
        [TestMethod]
        public async Task CheckIsFeedAvailableTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;
            var isErrored = systemUnderTest.IsErrored;

            // assert
            Assert.IsTrue(isAvailable);
            Assert.IsFalse(isErrored);
        }

        [TestMethod]
        public async Task CheckIsInvalidFeedUnavailableTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_invalidFeedUrl);

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;
            var isErrored = systemUnderTest.IsErrored;

            // assert
            Assert.IsFalse(isAvailable);
            Assert.IsTrue(isErrored);
        }

        [TestMethod]
        public async Task CheckFeedHasItemsTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var items = systemUnderTest.GetFeed();

            // assert
            Assert.IsNotNull(items);
        }

        [TestMethod]
        public async Task CheckFeedHasSixItemsTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var root = systemUnderTest.GetFeed();
            var numItems = root.Items.Count;

            // assert
            Assert.IsNotNull(root);
            Assert.AreEqual(6, numItems);
        }

        [TestMethod]
        public async Task CheckFeedHasUrlTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var urlFromFeedReader = systemUnderTest.Url;

            // assert
            Assert.IsNotNull(urlFromFeedReader);
            Assert.AreEqual(SpotlightBrowserTestOps.k_validFeedUrl, urlFromFeedReader);
        }
    }
}

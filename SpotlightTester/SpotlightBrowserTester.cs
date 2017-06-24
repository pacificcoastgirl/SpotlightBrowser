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

    // TODO: add tests for:
    // * large number of items in feed
    // * validating item fields
    // * caching

    [TestClass]
    public class SpotlightViewModelTests
    {
        // TODO: update this test to use fake data
        [TestMethod]
        public async Task SpotlightViewModelHasItemsTest()
        {
            // arrange
            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel();

            // act
            var items = systemUnderTest.Items;
            var hintText = systemUnderTest.HintText;

            // assert
            Assert.IsNotNull(items);
            //Assert.AreEqual(6, items.ToList().Count);
            Assert.AreEqual(string.Empty, hintText);
        }
        
        [TestMethod]
        public async Task SpotlightViewModelIsFeedAvailableTest()
        {
            // arrange
            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel();

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;
            var hintText = systemUnderTest.HintText;

            // assert
            Assert.IsTrue(isAvailable);
            Assert.AreEqual(string.Empty, hintText);
        }

        [TestMethod]
        public async Task SpotlightViewModelIsInvalidFeedUnavailableTest()
        {
            // arrange
            var mockReader = new Mock<IFeedReader<SpotlightItemRoot>>();
            mockReader.Setup(r => r.IsFeedAvailable).Returns(false);

            var systemUnderTest = await SpotlightViewModelFactory.CreateSpotlightViewModel(
                SpotlightBrowserTestOps.k_invalidFeedUrl,
                mockReader.Object);

            // act
            Assert.IsNotNull(systemUnderTest);
            var isAvailable = systemUnderTest.IsFeedAvailable;
            var hintText = systemUnderTest.HintText;

            // assert
            Assert.IsFalse(isAvailable);
            Assert.AreNotEqual(string.Empty, hintText);
        }
    }

    [TestClass]
    public class FeedReaderTests
    {
        [TestMethod]
        public async Task SpotlightReaderCheckIsFeedAvailableTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;

            // assert
            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public async Task SpotlightReaderCheckIsInvalidFeedUnavailableTest()
        {
            // arrange
            var mockCache = new Mock<IFeedCache<string>>();
            mockCache.Setup(c => c.IsFeedAvailable).Returns(false);

            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(
                SpotlightBrowserTestOps.k_invalidFeedUrl,
                mockCache.Object);

            // act
            var isAvailable = systemUnderTest.IsFeedAvailable;

            // assert
            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public async Task SpotlightReaderCheckFeedHasItemsTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var items = systemUnderTest.GetFeed();

            // assert
            Assert.IsNotNull(items);
        }

        // TODO: update this test to use fake data
        //[TestMethod]
        //public async Task SpotlightReaderCheckFeedHasSixItemsTest()
        //{
        //    // arrange
        //    var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

        //    // act
        //    var root = systemUnderTest.GetFeed();
        //    var numItems = root.Items.Count;

        //    // assert
        //    Assert.IsNotNull(root);
        //    Assert.AreEqual(6, numItems);
        //}

        [TestMethod]
        public async Task SpotlightReaderCheckFeedHasUrlTest()
        {
            // arrange
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(SpotlightBrowserTestOps.k_validFeedUrl);

            // act
            var urlFromFeedReader = systemUnderTest.Url;

            // assert
            Assert.IsNotNull(urlFromFeedReader);
            Assert.AreEqual(SpotlightBrowserTestOps.k_validFeedUrl, urlFromFeedReader);
        }

        [TestMethod]
        public async Task SpotlightReaderRetryTest()
        {
            // arrange
            var mockCache = new Mock<IFeedCache<string>>();
            mockCache.Setup(r => r.IsFeedAvailable).Returns(false);
            
            var systemUnderTest = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(
                SpotlightBrowserTestOps.k_invalidFeedUrl,
                mockCache.Object);

            // act
            var isAvailableBefore = systemUnderTest.IsFeedAvailable;
            systemUnderTest.Url = SpotlightBrowserTestOps.k_validFeedUrl;
            await systemUnderTest.RefreshFeedAsync();
            mockCache.Setup(r => r.IsFeedAvailable).Returns(true);
            
            var isAvailableAfter = systemUnderTest.IsFeedAvailable;
            var root = systemUnderTest.GetFeed();

            // assert
            Assert.IsFalse(isAvailableBefore);
            Assert.IsTrue(isAvailableAfter);
            Assert.IsNotNull(root);
        }
    }
}

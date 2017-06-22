using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpotlightBrowser;

namespace SpotlightBrowserTester
{
    [TestClass]
    public class SpotlightBrowserTester
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
    }
}

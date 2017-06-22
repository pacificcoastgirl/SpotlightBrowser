using System.Collections.Generic;

namespace SpotlightBrowser
{
    public class SpotlightViewModel
    {
        /// <summary>
        /// Returns whether the browser should operate in offline mode.
        /// </summary>
        public bool IsOffline { get { return false; } }

        /// <summary>
        /// The items in the feed.
        /// </summary>
        public IEnumerable<string> Items
        {
            get
            {
                return new List<string> { "one", "two", "three" };
            }
        }
    }
}

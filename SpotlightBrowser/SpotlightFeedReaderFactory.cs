using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public class SpotlightFeedReaderFactory
    {
        /// <summary>
        /// Constructor for creating a SpotlightFeedReader with a specified url. The reader
        /// will attempt to asynchronously fetch and cache the data.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<SpotlightFeedReader> CreateSpotlightFeedReader(string url)
        {
            return await SpotlightFeedReader.CreateAsync(url);
        }

        /// <summary>
        /// Overloaded constructor for creating a SpotlightFeedReader with a url and
        /// dependency injected cache. Mostly for unit testing purposes.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static async Task<SpotlightFeedReader> CreateSpotlightFeedReader(string url, IFeedCache<string> cache)
        {
            return await SpotlightFeedReader.CreateAsync(url, cache);
        }
    }
}

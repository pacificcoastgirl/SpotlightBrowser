using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public class SpotlightFeedCacheFactory
    {
        /// <summary>
        /// Constructor for creating a SpotlightFeedCache object, which abstracts
        /// the storage details of spotlight feed data.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<SpotlightFeedCache> CreateSpotlightFeedCache()
        {
            return await SpotlightFeedCache.CreateAsync();
        }

        /// <summary>
        /// Overloaded constructor for creating a SpotlightFeedCache object with a path
        /// to the file where data should be cached. Mostly for unit testing purposes.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<SpotlightFeedCache> CreateSpotlightFeedCache(string path)
        {
            return await SpotlightFeedCache.CreateAsync(path);
        }
    }
}

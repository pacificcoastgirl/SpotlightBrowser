
using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public interface IFeedCache<T>
    {
        /// <summary>
        /// Fetch the feed if it's present in the cache, and return it.
        /// If the cache does not contain the data, return null.
        /// </summary>
        T GetFeed();

        /// <summary>
        /// Save the contents of the feed to the cache.
        /// </summary>
        /// <returns></returns>
        Task PutFeedAsync(T data);

        /// <summary>
        /// Returns true if the feed is available, false if not.
        /// </summary>
        bool IsFeedAvailable { get; }

        /// <summary>
        /// Returns the file path where the cache will be saved to.
        /// </summary>
        string Path { get; }
    }
}

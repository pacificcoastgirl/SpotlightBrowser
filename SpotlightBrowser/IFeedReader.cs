using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public interface IFeedReader<T>
    {
        /// <summary>
        /// Asynchronously fetch the data at the location pointed to by Url, deserialize it
        /// and return it.
        /// </summary>
        Task<T> GetFeedAsync();

        /// <summary>
        /// The web location to fetch the feed from.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Returns whether this feed can be viewed.
        /// </summary>
        bool IsFeedAvailable { get; }
    }
}

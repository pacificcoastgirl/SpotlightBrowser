
using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public interface IFeedReader<T>
    {
        /// <summary>
        /// Fetch the data at the location pointed to by Url and return it.
        /// </summary>
        T GetFeed();

        /// <summary>
        /// Triggers a re-fetch of data.
        /// </summary>
        /// <returns></returns>
        Task RefreshFeedAsync();

        /// <summary>
        /// The web location to fetch the feed from.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Returns whether this feed can be viewed.
        /// </summary>
        bool IsFeedAvailable { get; }
    }
}

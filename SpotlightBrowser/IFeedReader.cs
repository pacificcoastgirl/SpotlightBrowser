
namespace SpotlightBrowser
{
    public interface IFeedReader<T>
    {
        /// <summary>
        /// Fetch the data at the location pointed to by Url and return it.
        /// </summary>
        T GetFeed();

        /// <summary>
        /// The web location to fetch the feed from.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Returns whether this feed can be viewed.
        /// </summary>
        bool IsFeedAvailable { get; }

        /// <summary>
        /// Returns whether an error was encountered trying to retrieve this feed.
        /// </summary>
        bool IsErrored { get; }
    }
}

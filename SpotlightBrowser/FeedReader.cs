using Newtonsoft.Json;
using System.Net;
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

    public class FeedReader
        : IFeedReader<SpotlightItemRoot>
    {
        private string m_url;
        
        public FeedReader(string url)
        {
            m_url = url;
        }

        public bool IsFeedAvailable
        {
            get { return true; }
        }

        public string Url
        {
            get { return m_url; }
        }

        public async Task<string> GetJson_(string url)
        {
            var task = Task.Run(() =>
            {
                string jsonString;

                using (WebClient wc = new WebClient())
                {
                    jsonString = wc.DownloadString(url);
                }

                return jsonString;
            });

            return await task;
        }
        
        public async Task<SpotlightItemRoot> GetFeedAsync()
        {
            var data = await GetJson_(m_url);
            return JsonConvert.DeserializeObject<SpotlightItemRoot>(data);
        }
    }
}

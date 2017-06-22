using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace SpotlightBrowser
{
    /// <summary>
    /// Implementation of IFeedReader that specializes on the root object
    /// in the spotlight feed's JSON specification.
    /// </summary>
    public class SpotlightFeedReader
        : IFeedReader<SpotlightItemRoot>
    {
        private string m_url;

        public SpotlightFeedReader(string url)
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

        public async Task<SpotlightItemRoot> GetFeedAsync()
        {
            var data = await GetJsonAsync_(m_url);
            return JsonConvert.DeserializeObject<SpotlightItemRoot>(data);
        }

        private async Task<string> GetJsonAsync_(string url)
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
    }
}

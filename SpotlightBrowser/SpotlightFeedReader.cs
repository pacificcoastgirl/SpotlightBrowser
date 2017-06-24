using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Collections;
using System;

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
        private string m_json;
        private bool m_isErrored;
        private object m_lock = new object();

        // Instances of this class must be created through the SpotlightViewModelFactory.
        private SpotlightFeedReader(string url)
        {
            m_url = url;
        }

        public bool IsFeedAvailable
        {
            get { return m_json != null; }
        }

        public bool IsErrored
        {
            get { return m_isErrored; }
        }

        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }
        
        // This method implements some rudimentary thread safety
        private async Task<SpotlightFeedReader> InitializeAsync_()
        {
            if (m_json == null)
            {
                var json = await LoadJsonFromCache_(m_url);
                if (json == null)
                {
                    json = await LoadJsonFromWeb_(m_url);
                    if (json == null)
                    {
                        m_isErrored = true;
                    }
                    else
                    {
                        lock (m_lock)
                        {
                            m_json = json;
                            m_isErrored = false;
                        }
                    }
                }
                else
                {
                    lock (m_lock)
                    {
                        m_json = json;
                        m_isErrored = false;
                    }
                }
            }

            return this;
        }

        public static Task<SpotlightFeedReader> CreateAsync(string url)
        {
            var reader = new SpotlightFeedReader(url);
            return reader.InitializeAsync_();
        }

        public async Task RefreshFeedAsync()
        {
            await InitializeAsync_();
        }

        public SpotlightItemRoot GetFeed()
        {
            if (!IsFeedAvailable) return null;

            return JsonConvert.DeserializeObject<SpotlightItemRoot>(m_json);
        }
        
        private async Task<string> LoadJsonFromWeb_(string url)
        {
            return await Task.Run(() =>
            {
                string json = null;

                var wc = new WebClient();

                try
                {
                    json = wc.DownloadString(url);
                }
                catch (Exception)
                {
                    json = null;
                }
                finally
                {
                    var disposable = wc as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }

                return json;
            });
        }

        private async Task<string> LoadJsonFromCache_(string url)
        {
            return await Task.Run(() =>
            {
                string json = null;

                // TODO: implement load from file system cache

                return json;
            });
        }
    }
}

using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace SpotlightBrowser
{
    /// <summary>
    /// Implementation of IFeedReader that returns the root object
    /// in the spotlight feed's JSON specification.
    /// </summary>
    public class SpotlightFeedReader
        : IFeedReader<SpotlightItemRoot>
    {
        private string m_url;
        private IFeedCache<string> m_cache;
        SpotlightItemRoot m_root;

        /// <summary>
        /// Create an instance of this object and initialize it asynchronously.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Task<SpotlightFeedReader> CreateAsync(string url)
        {
            var reader = new SpotlightFeedReader(url);
            return reader.InitializeAsync_();
        }

        /// <summary>
        /// Create an instance of this object with a url and caching object provided,
        /// and initialize it asynchronously.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Task<SpotlightFeedReader> CreateAsync(string url, IFeedCache<string> cache)
        {
            var reader = new SpotlightFeedReader(url);
            return reader.InitializeAsync_(cache);
        }

        // This call does not distinguish between a web error and a JSON
        // deserialize error. As an optimization, we could publish an error
        // enumeration detailing why the feed is unavailable for the view model
        // to present to the UI.
        public bool IsFeedAvailable
        {
            get { return m_cache.IsFeedAvailable; }
        }
        
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public async Task RefreshFeedAsync()
        {
            await InitializeAsync_();
        }

        public SpotlightItemRoot GetFeed()
        {
            return m_root;
        }

        // Instances of this class must be created through the SpotlightViewModelFactory.
        // This was done in order to facilitate asynchronous creation of the object.
        private SpotlightFeedReader(string url)
        {
            m_url = url;
        }

        private async Task<SpotlightFeedReader> InitializeAsync_()
        {
            return await InitializeAsync_(m_cache);
        }

        private async Task<SpotlightFeedReader> InitializeAsync_(IFeedCache<string> cache)
        {
            // DEBUGGING ONLY: a hardcoded delay to make sure the loading page is working correctly
            //await Task.Run(() =>
            //{
            //    Thread.Sleep(5000);
            //});

            // if the cache has not yet been initialized, initialize it
            if (cache == null)
            {
                cache = await SpotlightFeedCacheFactory.CreateSpotlightFeedCache();
            }

            m_cache = cache;

            SpotlightItemRoot root = null;

            // try to load the feed from the web first
            var json = await LoadJsonFromWeb_(m_url);
            if (json != null)
            {
                // try to deserialize
                root = await DeserializeJson_(json);
                if (root == null)
                {
                    // invalid json, clear it and try to fetch from cache
                    json = null;
                }
                else
                {
                    // store the result back in cache; at this point we know we have
                    // valid json
                    await cache.PutFeedAsync(json);
                }
            }

            // if we didn't get a valid feed from the web, try to fetch from cache
            if (json == null)
            {
                json = cache.GetFeed();
                if (json != null)
                {
                    // try to deserialize
                    root = await DeserializeJson_(json);
                    if (root == null)
                    {
                        json = null;
                    }
                }
            }

            m_root = root;

            return this;
        }

        // Tries to deserialize the specified JSON string, and returns null if the string
        // cannot be deserialized
        private async Task<SpotlightItemRoot> DeserializeJson_(string json)
        {
            SpotlightItemRoot root = null;
            if (json != null)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        root = JsonConvert.DeserializeObject<SpotlightItemRoot>(json);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to deserialize JSON");
                        root = null;
                    }
                });
            }

            return root;
        }

        // Tries to fetch the JSON string at the specified url
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
                    Console.WriteLine("Failed to download string from url {0}", url);
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
    }
}

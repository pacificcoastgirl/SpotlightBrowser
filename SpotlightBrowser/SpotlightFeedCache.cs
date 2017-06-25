
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpotlightBrowser
{
    /// <summary>
    /// This class is responsible for data caching and local storage. It is agnostic
    /// to the semantics of the data being stored.
    /// 
    /// Note: Since we are guaranteed atomicity of strings in C#, there is no
    /// need to lock on assignment operations. We could consider using Interlocked.CompareExchange
    /// for bulletproof thread safety, as an optimization.
    /// </summary>
    public class SpotlightFeedCache : IFeedCache<string>
    {
        private string m_cachedJson;
        private object m_lock = new object();
        private static string k_defaultCachePath = "D:\\spotlight_cache.txt";
        private string m_path;

        public static Task<SpotlightFeedCache> CreateAsync()
        {
            var cache = new SpotlightFeedCache();
            return cache.InitializeAsync_();
        }

        public static Task<SpotlightFeedCache> CreateAsync(string path)
        {
            var cache = new SpotlightFeedCache();
            return cache.InitializeAsync_(path);
        }

        public string Path { get { return m_path; } }

        public string GetFeed()
        {
            return m_cachedJson;
        }

        public async Task PutFeedAsync(string data)
        {
            m_cachedJson = data;
            await SaveAsync_();
        }

        public bool IsFeedAvailable
        {
            get
            {
                var json = m_cachedJson;
                return json != null;
            }
        }

        // Instances of this class must be created through the SpotlightViewModelFactory.
        // This was done in order to facilitate asynchronous creation of the object.
        private SpotlightFeedCache()
        { }

        private async Task<SpotlightFeedCache> InitializeAsync_()
        {
            return await InitializeAsync_(m_path);
        }

        private async Task<SpotlightFeedCache> InitializeAsync_(string path)
        {
            if (path == null)
            {
                path = k_defaultCachePath;
            }

            m_path = path;
            await LoadAsync_();
            return this;
        }

        /// <summary>
        /// Populate the cache from disk. Relies on file system locking
        /// to throw exceptions when concurrent read/writes are attempted.
        /// </summary>
        private async Task LoadAsync_()
        {
            string data;
            StreamReader file = null;

            try
            {
                file = new StreamReader(m_path);

                data = await file.ReadToEndAsync();

                // replace whatever's been cached locally
                m_cachedJson = data;
            }
            catch (Exception)
            {
                Console.WriteLine("File read from {0} failed!", m_path);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }

        /// <summary>
        /// Persist the cache to disk. Relies on file system locking
        /// to throw exceptions when concurrent read/writes are attempted.
        /// </summary>
        /// <returns></returns>
        private async Task SaveAsync_()
        {
            StreamWriter file = null;

            try
            {
                file = File.CreateText(m_path);
                await file.WriteAsync(m_cachedJson);
            }
            catch (Exception)
            {
                Console.WriteLine("File write to {0} failed!", m_path);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }
    }
}

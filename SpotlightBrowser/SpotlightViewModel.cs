using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SpotlightBrowser
{
    /// <summary>
    /// Main view model for the spotlight browser container, with properties representing state
    /// information and a bindable collection of items representing movies, tv series and collections
    /// in the spotlight feed.
    /// </summary>
    public class SpotlightViewModel
        : ViewModelBase
    {
        private static string k_defaultSpotlightFeedUrl = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
        private static string k_offlineHintText = "We're sorry, you appear to be offline.\nAfter you fix your network collection, you can retry here.";
        private SpotlightFeedReader m_reader;
        private IEnumerable<SpotlightItemViewModel> m_items;
        
        // Instances of this class must be created through the SpotlightViewModelFactory.
        private SpotlightViewModel()
        { }

        /// <summary>
        /// Returns hint text for displaying user-relevant status information.
        /// </summary>
        public string Hint
        {
            get
            {
                if (IsFeedErrored)
                {
                    return k_offlineHintText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Returns whether the feed is available for viewing.
        /// </summary>
        public bool IsFeedAvailable { get { return m_reader.IsFeedAvailable; } }

        /// <summary>
        /// Returns whether the feed has encountered any errors in loading.
        /// </summary>
        public bool IsFeedErrored { get { return m_reader.IsErrored; } }

        public static async Task<SpotlightViewModel> CreateAsync()
        {
            return await CreateAsync(k_defaultSpotlightFeedUrl);
        }

        public static async Task<SpotlightViewModel> CreateAsync(string url)
        {
            var vm = new SpotlightViewModel();
            return await vm.InitializeAsync_(url);
        }

        /// <summary>
        /// Returns the items contained in the feed.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SpotlightItemViewModel> GetItems_()
        {
            var root = m_reader.GetFeed();
            if (root == null) return null;

            return root.Items.Select(i => new SpotlightItemViewModel(i.Title, i.Description, i.ImageUrl, i.Id, i.ItemType));
        }

        /// <summary>
        /// The items in the feed.
        /// </summary>
        public IEnumerable<SpotlightItemViewModel> Items
        {
            get
            {
                // Cache the items in memory. This is not scalable.
                if (m_items == null)
                {
                    m_items = GetItems_();
                }

                return m_items;
            }
        }

        private async Task<SpotlightViewModel> InitializeAsync_(string url)
        {
            m_reader = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(url);
            return this;
        }
    }
}

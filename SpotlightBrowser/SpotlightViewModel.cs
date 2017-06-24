using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private static string k_offlineHintText = "We're sorry, you appear to be offline.\nAfter you fix your network connection, you can retry here.";
        
        private IFeedReader<SpotlightItemRoot> m_reader;
        private IEnumerable<SpotlightItemViewModel> m_items;
        private string k_retryFetchFeedText = "Retry";
        private IAsyncCommand m_retryCommand;
        private bool m_isLoaded;

        public static async Task<SpotlightViewModel> CreateAsync()
        {
            return await CreateAsync(k_defaultSpotlightFeedUrl, null);
        }

        public static Task<SpotlightViewModel> CreateAsync(string url, IFeedReader<SpotlightItemRoot> reader)
        {
            var vm = new SpotlightViewModel();
            return vm.InitializeAsync_(url, reader);
        }

        /// <summary>
        /// Returns hint text for displaying user-relevant status information.
        /// </summary>
        public string HintText
        {
            get
            {
                return IsFeedAvailable
                        ? string.Empty
                        : k_offlineHintText;
            }
        }

        /// <summary>
        /// Returns whether the feed is available for viewing.
        /// </summary>
        public IAsyncCommand RetryCommand { get { return m_retryCommand; } }

        /// <summary>
        /// Returns whether the feed is available for viewing.
        /// </summary>
        public string RetryText { get { return k_retryFetchFeedText; } }
        
        /// <summary>
        /// Returns whether the feed is available for viewing.
        /// </summary>
        public bool IsFeedAvailable { get { return m_reader.IsFeedAvailable; } }
        
        /// <summary>
        /// Returns whether the feed has been loaded.
        /// </summary>
        public bool IsFeedLoaded
        {
            get { return m_isLoaded; }
            set
            {
                SetValue_(ref m_isLoaded, value, () => IsFeedLoaded);
            }
        }

        /// <summary>
        /// The web location to fetch the feed from.
        /// </summary>
        public string Url
        {
            get { return m_reader.Url; }
            set { m_reader.Url = value; }
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
                    IsFeedLoaded = false;
                    m_items = GetItems_();
                    IsFeedLoaded = true;
                }

                OnPropertyChanged_(() => IsFeedAvailable);

                return m_items;
            }
        }

        // Instances of this class must be created through the SpotlightViewModelFactory.
        private SpotlightViewModel()
        {
            m_retryCommand = new AwaitableDelegateCommand(OnRetryFetchFeed_);
        }

        private async Task OnRetryFetchFeed_()
        {
            // refresh the data source
            await m_reader.RefreshFeedAsync();

            // clear the items list
            m_items = null;

            OnPropertyChanged_(() => Items);
        }

        private IEnumerable<SpotlightItemViewModel> GetItems_()
        {
            var root = m_reader.GetFeed();
            if (root == null || root.Items == null) return null;

            return root.Items.Select(i => new SpotlightItemViewModel(i.Title, i.Description, i.ImageUrl, i.Id, i.ItemType));
        }

        private async Task<SpotlightViewModel> InitializeAsync_(string url, IFeedReader<SpotlightItemRoot> reader)
        {
            if (reader == null)
            {
                reader = await SpotlightFeedReaderFactory.CreateSpotlightFeedReader(url);
            }

            m_reader = reader;

            return this;
        }
    }
}

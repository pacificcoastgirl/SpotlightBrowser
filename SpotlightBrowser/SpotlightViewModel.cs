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
        private static string k_spotlightFeedUrl = "https://mediadiscovery.microsoft.com/v1.0/channels/video.spotlight?languages=en&market=US&storeVersion=10.17054.13511.0&clientType=MsVideo&deviceFamily=Windows.Desktop";
        private SpotlightFeedReader m_reader;
        private IEnumerable<SpotlightItemViewModel> m_items;

        public SpotlightViewModel()
            : this(k_spotlightFeedUrl)
        { }

        public SpotlightViewModel(string url)
        {
            m_reader = new SpotlightFeedReader(url);
        }

        /// <summary>
        /// Returns whether the browser is operating in offline mode.
        /// </summary>
        public bool IsOffline { get { return false; } }

        /// <summary>
        /// Returns whether the feed has been loaded.
        /// </summary>
        public bool IsFeedLoaded
        {
            get { return true; }
        }

        /// <summary>
        /// Returns whether the feed has encountered any errors in loading.
        /// </summary>
        public bool IsFeedErrored { get { return false; } }

        /// <summary>
        /// Asynchronously returns the items contained in the feed.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SpotlightItemViewModel>> GetItemsAsync()
        {
            var root = await m_reader.GetFeedAsync();
            return root.Items.Select(i => new SpotlightItemViewModel(i.Title, i.Description, i.ImageUrl, i.Id, i.ItemType));
        }

        /// <summary>
        /// The items in the feed.
        /// </summary>
        public IEnumerable<SpotlightItemViewModel> Items
        {
            get
            {
                if (m_items == null)
                {
                    if (m_reader.IsFeedAvailable)
                    {
                        Application.Current.Dispatcher.InvokeAsync(async () =>
                        {
                            Items = await GetItemsAsync();
                        });
                    }
                }

                return m_items;
            }
            set
            {
                if (value != m_items)
                {
                    m_items = value;
                    OnPropertyChanged_(() => Items);
                }
            }
        }
    }
}

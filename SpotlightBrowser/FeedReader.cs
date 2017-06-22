using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public interface IFeedReader
    {
        Task<IEnumerable<SpotlightItem>> GetAllItemsInFeed();
        Task<SpotlightItem> GetNextItemInFeed();
        bool IsFeedAvailable { get; set; }
    }

    public class FeedReader
        : IFeedReader
    {
        public bool IsFeedAvailable
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Task<IEnumerable<SpotlightItem>> GetAllItemsInFeed()
        {
            throw new NotImplementedException();
        }

        public Task<SpotlightItem> GetNextItemInFeed()
        {
            throw new NotImplementedException();
        }
    }
}

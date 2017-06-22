using System.Collections.Generic;

namespace SpotlightBrowser
{
    public class SpotlightItemRoot
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public SpotlightItemInfo PagingInfo { get; set; }
        public List<SpotlightItem> Items { get; set; }
    }

    public class SpotlightItemInfo
    {
        public int TotalItems { get; set; }
    }
}

namespace SpotlightBrowser
{
    public enum SpotlightFeedItemType
    {
        Movie,
        Series,
        Collection
    }

    public class SpotlightItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public SpotlightFeedItemType ItemType { get; set; }
    }
}

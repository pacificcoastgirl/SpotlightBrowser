namespace SpotlightBrowser
{
    public enum ItemType
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
        public ItemType ItemType { get; set; }
    }
}

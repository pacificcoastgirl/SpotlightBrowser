
namespace SpotlightBrowser
{
    /// <summary>
    /// Bindable object representing an item in the spotlight feed. 
    /// </summary>
    public class SpotlightItemViewModel 
        : ViewModelBase
    {
        private string m_title;
        private string m_description;
        private string m_imageUrl;
        private string m_id;
        private ItemType m_itemType;

        public SpotlightItemViewModel(
            string title, 
            string description, 
            string imageUrl,
            string id,
            ItemType itemType)
        {
            m_title = title;
            m_description = description;
            m_imageUrl = imageUrl;
            m_id = id;
            m_itemType = itemType;
        }

        public string Title
        {
            get
            {
                return m_title;
            }
        }

        public string Description
        {
            get
            {
                return m_description;
            }
        }

        public string ImageUrl
        {
            get
            {
                return m_imageUrl;
            }
        }

        public string Id
        {
            get
            {
                return m_id;
            }
        }

        public ItemType ItemType
        {
            get
            {
                return m_itemType;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ItemType ItemType { get; }
        public string Title { get; }
        public string Description { get; }
        public Uri Url { get; }
        public int Id { get; }
    }
}

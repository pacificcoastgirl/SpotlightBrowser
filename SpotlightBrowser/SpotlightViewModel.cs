using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public class SpotlightViewModel
    {
        public IEnumerable<string> Items
        {
            get
            {
                return new List<string> { "one", "two", "three" };
            }
        }
    }
}

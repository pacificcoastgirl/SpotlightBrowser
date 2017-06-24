using System.Threading.Tasks;

namespace SpotlightBrowser
{
    public class SpotlightViewModelFactory
    {
        /// <summary>
        /// Overloaded constructor for creating a SpotlightViewModel with the default url.
        /// </summary>
        /// <returns></returns>
        public static async Task<SpotlightViewModel> CreateSpotlightViewModel()
        {
            return await SpotlightViewModel.CreateAsync();
        }

        /// <summary>
        /// Overload for creating a SpotlightViewModel with a specific url and dependency
        /// injected reader. Mostly for unit testing purposes.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<SpotlightViewModel> CreateSpotlightViewModel(string url, IFeedReader<SpotlightItemRoot> reader)
        {
            return await SpotlightViewModel.CreateAsync(url, reader);
        }
    }
}

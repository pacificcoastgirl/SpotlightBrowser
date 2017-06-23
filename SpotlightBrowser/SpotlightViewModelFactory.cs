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
        /// Overload for creating a SpotlightViewModel with a specific url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<SpotlightViewModel> CreateSpotlightViewModel(string url)
        {
            return await SpotlightViewModel.CreateAsync(url);
        }
    }
}

using System.Windows;

namespace SpotlightBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SpotlightView : Window
    {
        public SpotlightView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // A better solution here would decouple the view model from the view via
            // dependency injection, but I took the simpler approach here in letting 
            // the view be aware of the view model (but not vice versa, to enable unit
            // testing)
            DataContext = await SpotlightViewModelFactory.CreateSpotlightViewModel();
        }
    }
}

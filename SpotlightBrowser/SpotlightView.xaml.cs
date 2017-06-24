using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SpotlightBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SpotlightView : Window
    {
        DispatcherTimer m_dispatcherTimer;

        public SpotlightView()
        {
            InitializeComponent();
            FlipView.HideControlButtons();

            // it's simpler to handle this in the code behind than in XAML, 
            // and it doesn't belong in the view model as it's a detail of the view
            m_dispatcherTimer = new DispatcherTimer();
            m_dispatcherTimer.Tick += OnDispatcherTimerTimeElapsed_;
            m_dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            m_dispatcherTimer.Start();
        }

        private void OnDispatcherTimerTimeElapsed_(object sender, EventArgs e)
        {
            if (FlipView.SelectedIndex == FlipView.Items.Count - 1)
            {
                FlipView.SelectedIndex = 0;
            }
            else
            {
                FlipView.SelectedIndex++;
            }
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

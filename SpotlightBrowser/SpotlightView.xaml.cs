using MahApps.Metro.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SpotlightBrowser
{
    /// <summary>
    /// Interaction logic for SpotlightView.xaml
    /// </summary>
    public partial class SpotlightView : Window
    {
        private DispatcherTimer m_dispatcherTimer;
        private readonly TimeSpan m_timePerPageSeconds = new TimeSpan(0, 0, 10);
        
        public SpotlightView()
        {
            InitializeComponent();

            // The FlipView control does not provide a two-way bindable selection,
            // so we'll need to modify it in the code behind
            FlipView.ShowControlButtons();
            FlipView.SelectionChanged += OnFlipViewSelectionChanged_;

            // Start the progress bar, and animate the duration to make it appear smooth
            ProgressBar.SetPercent(100, m_timePerPageSeconds);

            // create a timer which ticks every ten seconds, at which point we turn the 
            // page in the FlipView
            m_dispatcherTimer = new DispatcherTimer();

            // we don't really care about detaching the event handler, since the timer persists
            // for the lifetime of the application
            m_dispatcherTimer.Tick += OnDispatcherTimerTimeElapsed_;
            m_dispatcherTimer.Interval = m_timePerPageSeconds;
            m_dispatcherTimer.Start();
        }

        private void OnFlipViewSelectionChanged_(object sender, SelectionChangedEventArgs e)
        {
            ProgressBar.SetPercent(0, TimeSpan.Zero);

            m_dispatcherTimer.Stop();
            m_dispatcherTimer.Start();

            // we have to clear the animation here, as we can't update the value while it's
            // still animating
            ProgressBar.BeginAnimation(MetroProgressBar.ValueProperty, null);

            // restart the animation
            ProgressBar.SetPercent(100, m_timePerPageSeconds);
        }

        // Update the FlipView's current page
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
            // dependency injection (easily facilitated by a framework like Prism), 
            // but I took the simpler and acceptable approach here in letting the view 
            // be aware of the view model (but not vice versa, to maintain some separation
            // of concerns)
            DataContext = await SpotlightViewModelFactory.CreateSpotlightViewModel();
        }
    }
}

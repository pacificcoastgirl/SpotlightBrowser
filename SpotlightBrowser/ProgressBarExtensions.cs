
using MahApps.Metro.Controls;
using System;
using System.Windows.Media.Animation;

namespace SpotlightBrowser
{
    public static class ProgressBarExtensions
    {
        /// <summary>
        /// A simple extension method for animating progress smoothly.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="percentage"></param>
        /// <param name="delay"></param>
        public static void SetPercent(this MetroProgressBar progressBar, double percentage, TimeSpan delay)
        {
            DoubleAnimation animation = new DoubleAnimation(percentage, delay);
            progressBar.BeginAnimation(MetroProgressBar.ValueProperty, animation);
        }
    }
}

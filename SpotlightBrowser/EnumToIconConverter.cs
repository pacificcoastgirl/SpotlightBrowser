using System;
using System.Windows.Data;

namespace SpotlightBrowser
{
    public class EnumToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            switch ((SpotlightFeedItemType)value)
            {
                case SpotlightFeedItemType.Collection:
                    return "/Resources/collectionicon.png";
                case SpotlightFeedItemType.Movie:
                    return "/Resources/movieicon.png";
                case SpotlightFeedItemType.Series:
                    return "/Resources/tvicon.png";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

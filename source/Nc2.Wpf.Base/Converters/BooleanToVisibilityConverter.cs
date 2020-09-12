namespace Nc2.Wpf.Base.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Boolean Invert { get; set; }

        public Boolean Collapsed { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is Boolean isTrue && isTrue == Invert)
            {
                return Collapsed ? Visibility.Collapsed : Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible != Invert;
        }
    }
}

namespace Nc2.Wpf.Base.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class BooleanToColorConverter : IValueConverter
    {
        public BooleanToColorConverter()
        {
            TrueColor = Brushes.Black;
            FalseColor = Brushes.White;
            NullColor = Brushes.Gray;
        }

        public Boolean Invert { get; set; }
        public SolidColorBrush NullColor { get; set; }
        public SolidColorBrush FalseColor { get; set; }
        public SolidColorBrush TrueColor { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is Boolean isTrue)
            {
                return isTrue == Invert ? FalseColor : TrueColor;
            }

            return NullColor;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                if (brush == NullColor)
                {
                    return null;
                }

                return brush == TrueColor;
            }

            return false;
        }
    }
}

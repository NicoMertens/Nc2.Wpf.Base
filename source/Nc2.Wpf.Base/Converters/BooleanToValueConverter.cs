namespace Nc2.Wpf.Base.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BooleanToValueConverter : IValueConverter
    {
        public Boolean Invert { get; set; }
        public Object NullValue { get; set; }
        public Object FalseValue { get; set; }
        public Object TrueValue { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is Boolean isTrue)
            {
                return isTrue == Invert ? FalseValue : TrueValue;
            }

            return NullValue;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == NullValue)
            {
                return null;
            }

            return value == TrueValue;
        }
    }
}

using System;
using System.Globalization;
using Xamarin.Forms;

namespace RestaurantApp.Converters
{
    public class BoolToBarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            var isselected = (bool)value;
            if (isselected)
            {
                return Color.Yellow;
            }
            return Color.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

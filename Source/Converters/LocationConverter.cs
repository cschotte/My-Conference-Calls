using System;
using System.Windows.Data;
using System.Globalization;
using MyConferenceCalls.Resources;

namespace MyConferenceCalls.Converters
{
    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return null;

            return string.Format(AppResources.Location, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

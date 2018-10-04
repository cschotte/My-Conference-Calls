using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;
using Microsoft.Phone.UserData;

namespace MyConferenceCalls.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                AppointmentStatus status = (AppointmentStatus)value;

                if (status == AppointmentStatus.Tentative)
                {
                    return Application.Current.Resources["TransparentBrush"] as Brush;
                }
            }
            catch (Exception)
            {
                return Application.Current.Resources["PhoneAccentBrush"] as Brush;
            }

            return Application.Current.Resources["PhoneAccentBrush"] as Brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

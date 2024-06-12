using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class DiscoveredDeviceCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var devices = (int)value;

            if (devices > 0)
            {
                return true;
            }
            else
                return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

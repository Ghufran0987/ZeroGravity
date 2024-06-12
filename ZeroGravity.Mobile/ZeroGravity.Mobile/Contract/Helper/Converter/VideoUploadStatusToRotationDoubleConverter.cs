using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class VideoUploadStatusToRotationDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double rotation = 0;

            if (value == null)
            {
                return rotation;
            }

            var stringValue = value.ToString();

            if (Enum.TryParse(stringValue, out VideoUploadStatus uploadStatus))
            {
                switch (uploadStatus)
                {
                    case VideoUploadStatus.Uploading:
                        rotation = 70;
                        break;
                    
                    default:
                        // alle anderen
                        rotation = 0;
                        break;
                }
            }

            return rotation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

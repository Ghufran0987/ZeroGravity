using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Resources.Fonts;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class VideoUploadStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = CustomColors.DarkGray;

            if (value == null)
            {
                return color;
            }

            var stringValue = value.ToString();

            if (Enum.TryParse(stringValue, out VideoUploadStatus uploadStatus))
            {
                switch (uploadStatus)
                {
                    case VideoUploadStatus.Finished:
                        color = CustomColors.Green;
                        break;
                    case VideoUploadStatus.Other:
                        color = CustomColors.Yellow;
                        break;
                    case VideoUploadStatus.Error:
                        color = CustomColors.Red;
                        break;
                    
                    default:
                        // alle anderen
                        color = CustomColors.DarkGray;
                        break;
                }
            }


            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

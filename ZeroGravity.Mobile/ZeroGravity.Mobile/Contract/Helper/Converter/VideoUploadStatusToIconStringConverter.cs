using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class VideoUploadStatusToIconStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out VideoUploadStatus uploadStatus))
                    switch (uploadStatus)
                    {
                        case VideoUploadStatus.Idle:
                            iconString = "\uf0ee";
                            break;
                        case VideoUploadStatus.Uploading:
                            iconString = "\uf1ce";
                            break;
                        case VideoUploadStatus.Finished:
                            iconString = "\uf058";
                            break;

                        default:
                            iconString = "";
                            break;
                    }
            }

            return iconString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

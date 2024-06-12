using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Resources.Fonts;
using ZeroGravity.Shared.Enums;
using Color = System.Drawing.Color;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class FeedbackStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Color.LightSlateGray;

            if (value == null) return color;

            var stringValue = value.ToString();

            if (!Enum.TryParse(stringValue, out FeedbackState feedbackState)) return color;
            //if (Enum.TryParse(parameter as string, out FeedbackColorType feedbackColorType)) return color;

            var feedbackColorType = (FeedbackColorType)parameter;

            switch (feedbackColorType)
            {
                case FeedbackColorType.Healthy:
                    switch (feedbackState)
                    {
                        case FeedbackState.TooLow:
                            color = CustomColors.Pink;
                            break;
                        case FeedbackState.VeryLow:
                            color = CustomColors.Orange;
                            break;
                        case FeedbackState.LittleLow:
                            color = CustomColors.LightGreen;
                            break;
                        case FeedbackState.Perfect:
                            color = CustomColors.NewGreen;
                            break;
                        case FeedbackState.LittleHigh:
                            color = CustomColors.LightGreen;
                            break;
                        case FeedbackState.VeryHigh:
                            color = CustomColors.Orange;
                            break;
                        case FeedbackState.TooHigh:
                            color = CustomColors.Pink;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case FeedbackColorType.Unhealthy:

                    switch (feedbackState)
                    {
                        case FeedbackState.TooLow:
                            color = Color.Green;
                            break;
                        case FeedbackState.VeryLow:
                            color = CustomColors.LightGreen;
                            break;
                        case FeedbackState.LittleLow:
                            color = CustomColors.LightGreen;
                            break;
                        case FeedbackState.Perfect:
                            color = CustomColors.Orange;
                            break;
                        case FeedbackState.LittleHigh:
                            color = CustomColors.Orange;
                            break;
                        case FeedbackState.VeryHigh:
                            color = CustomColors.Pink;
                            break;
                        case FeedbackState.TooHigh:
                            color = CustomColors.Pink;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }



            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
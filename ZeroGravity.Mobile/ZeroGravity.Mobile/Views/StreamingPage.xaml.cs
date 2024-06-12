using System;
using System.ComponentModel;
using System.Globalization;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StreamingPage : IStreamingPage
    {
        public StreamingPage()
        {
            InitializeComponent();
        }

        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (sender is MediaElement me)
            //{
            //    if (e.PropertyName == nameof(me.Width))
            //    {
            //        me.HeightRequest = me.Width / 9 * 16;
            //    }
            //}
        }
    }

    public class VideoIdToUrlConverter : IMarkupExtension, IValueConverter
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new VideoIdToUrlConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return "https://video-library.showheroes.com/files/videos/" + s;
            }

            return BindableProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.Extensions
{
    [ContentProperty(nameof(Source))]

    public class ImageSourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;

            return ImageSource.FromResource(Source, typeof(ImageSourceExtension).GetTypeInfo().Assembly);
        }
    }
}

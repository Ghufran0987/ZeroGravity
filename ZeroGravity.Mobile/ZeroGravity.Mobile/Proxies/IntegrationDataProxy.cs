using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class IntegrationDataProxy : ProxyBase
    {
        private int _integrationType;
        private bool _isLinked;
        private string _name;
        private ImageSource _image;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int IntegrationType
        {
            get => _integrationType;
            set => SetProperty(ref _integrationType, value);
        }

        public bool IsLinked
        {
            get => _isLinked;
            set => SetProperty(ref _isLinked, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
    }
}
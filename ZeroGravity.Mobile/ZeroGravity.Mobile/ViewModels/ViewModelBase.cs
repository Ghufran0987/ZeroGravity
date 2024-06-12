using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Events;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ViewModelBase : BindableBase, INavigatedAware, IDestructible, IApplicationLifecycleAware
    {
        protected IVmCommonService Service;
        private string _title;
        private bool _isBusy;
        private bool _showProgress;
        private readonly bool _checkInternet;
        private UriImageSource _productImage;

        public UriImageSource ProductImage
        {
            get => _productImage;
            set => SetProperty(ref _productImage, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool ShowProgress
        {
            get => _showProgress;
            set => SetProperty(ref _showProgress, value);
        }

        private bool _hasInternetConnection;

        public bool HasInternetConnection
        {
            get { return _hasInternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet; }
            set { SetProperty(ref _hasInternetConnection, value); }
        }

        private bool _isLoadingImageBusy;
        public bool IsLoadingImageBusy
        {
            get => _isLoadingImageBusy;
            set => SetProperty(ref _isLoadingImageBusy, value);
        }

        public ViewModelBase(IVmCommonService service, bool checkInternet = true)
        {
            Service = service;
            _checkInternet = checkInternet;
        }

        public virtual async void OnNavigatedTo(INavigationParameters parameters)
        {
            //if (!_checkInternet)
            //{
            //    return;
            //}

            //Connectivity.ConnectivityChanged -= ConnectivityChanged;

            //_hasInternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet;

            //Connectivity.ConnectivityChanged += ConnectivityChanged;

            //if (!HasInternetConnection)
            //{
            //    Service.EventAggregator.GetEvent<NoInternetConnectionEvent>().Publish();
            //}
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            switch (e.NetworkAccess)
            {
                case NetworkAccess.Unknown:
                    // Unable to determine internet connectivity.
                    HasInternetConnection = false;
                    break;

                case NetworkAccess.None:
                    // No connectivity is available.
                    HasInternetConnection = false;
                    break;

                case NetworkAccess.Local:
                    HasInternetConnection = false;
                    // Local network access only.
                    break;

                case NetworkAccess.ConstrainedInternet:
                    // Limited internet access.
                    // Indicates captive portal connectivity, where local access to a web portal is provided,
                    // but access to the Internet requires that specific credentials are provided via a portal.
                    HasInternetConnection = false;
                    break;

                case NetworkAccess.Internet:
                    // Local and internet access
                    HasInternetConnection = true;
                    break;
            }

            // Limitations
            /*
             * It is important to note that it is possible that Internet is reported by NetworkAccess
             * but full access to the web is not available.
             * Due to how connectivity works on each platform it can only guarantee that a connection is available.
             * For instance the device may be connected to a Wi-Fi network, but the router is disconnected from the internet.
             * In this instance Internet may be reported, but an active connection is not available.
             *
             * https://docs.microsoft.com/de-de/xamarin/essentials/connectivity
             */
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnSleep()
        {
        }
    }
}
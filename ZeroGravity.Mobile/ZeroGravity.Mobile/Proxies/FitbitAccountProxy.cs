using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class FitbitAccountProxy : ProxyBase
    {
        private int _accountId;

        private string _authenticationUrl;

        private string _token;

        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }

        public string AuthenticationUrl
        {
            get => _authenticationUrl;
            set => SetProperty(ref _authenticationUrl, value);
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }
    }
}
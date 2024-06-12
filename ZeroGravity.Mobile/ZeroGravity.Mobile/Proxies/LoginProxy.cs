using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class LoginProxy : ProxyBase
    {
        private string _email;
        private string _password;
        private bool _saveLogin;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool SaveLogin
        {
            get => _saveLogin;
            set => SetProperty(ref _saveLogin, value);
        }
    }
}

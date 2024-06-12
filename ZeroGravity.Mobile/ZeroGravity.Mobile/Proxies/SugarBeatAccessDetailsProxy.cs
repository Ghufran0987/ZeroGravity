using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;

namespace ZeroGravity.Mobile.Proxies
{
    public class SugarBeatAccessDetailsProxy : BindableBase
    {
        private string _address;
        private string _password;

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}

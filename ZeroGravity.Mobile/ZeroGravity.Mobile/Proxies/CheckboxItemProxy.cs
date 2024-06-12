using Prism.Mvvm;

namespace ZeroGravity.Mobile.Proxies
{
    public class CheckboxItemProxy : BindableBase
    {
        private bool _isChecked;
        private int _key;

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public int Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }
    }
}
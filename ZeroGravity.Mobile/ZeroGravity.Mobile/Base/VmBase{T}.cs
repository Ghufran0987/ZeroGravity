using System;
using System.Windows.Input;
using Prism.Commands;
using Syncfusion.SfGauge.XForms;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Base
{
    public abstract class VmBase<TPage> : ViewModelBase, IVm<TPage> where TPage : IPage
    {
        protected TPage Page;
        protected VmBase(IVmCommonService service, bool checkInternet = true) : base(service, checkInternet)
        {
            CloseOverlayCommand = new DelegateCommand<object>(CloseOverlay);
        }

        public void SetPage(TPage page)
        {
            Page = page;
        }

        public DelegateCommand<object> CloseOverlayCommand { get; }
        
        public void OpenOverlay()
        {
            if (Page == null) return;

            Page.OpenOverlay();
        }


        internal virtual void OnDailyCloseOverlay()
        {

        }

        protected virtual void OnCustomCloseOverlay()
        {
            
        }

        private void CloseOverlay(object customAction)
        {
            if (Page == null) return;

            if (customAction is string s && 
                !string.IsNullOrEmpty(s))
            {
                OnCustomCloseOverlay();
            }
            
            Page.CloseOverlay();
        }
    }
}

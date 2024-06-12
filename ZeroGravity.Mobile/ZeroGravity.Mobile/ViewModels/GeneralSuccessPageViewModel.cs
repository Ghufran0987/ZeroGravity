using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class GeneralSuccessPageViewModel : VmBase<IGeneralSuccessPage, IGeneralSuccessPageVmProvider, GeneralSuccessPageViewModel>
    {
        public GeneralSuccessPageViewModel(IVmCommonService service, IGeneralSuccessPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var navParams = NavigationParametersHelper.GetNavigationParameters<GeneralSuccessNavParams>(parameters);
            if (navParams != null)
            {
                Title = navParams.Title;
                Text = navParams.Text;
                SubText = navParams.SubText;
                IconUnicode = navParams.IconUnicode;
            }
            else
            {
                Text = AppResources.Common_Success;
                IconUnicode = "\uf00c";
            }

        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _subText;
        public string SubText
        {
            get => _subText;
            set => SetProperty(ref _subText, value);
        }

        private string _iconUnicode;

        public string IconUnicode
        {
            get => _iconUnicode;
            set => SetProperty(ref _iconUnicode, value);
        }

    }
}

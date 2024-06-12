using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ResultPageViewModel : VmBase<IResultPage, IResultPageVmProvider, ResultPageViewModel>
    {
        private bool _result;
        private string _message;
        private string _iconUnicode;

        public ResultPageViewModel(
            IVmCommonService service, 
            IResultPageVmProvider provider, 
            ILoggerFactory loggerFactory, 
            IApiService apiService, 
            bool checkInternet = true) : base(service, provider, loggerFactory, apiService, checkInternet)
        {
        }

        public bool Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string IconUnicode
        {
            get => _iconUnicode;
            set => SetProperty(ref _iconUnicode, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var navParams = NavigationParametersHelper.GetNavigationParameters<PageNavigationParams>(parameters);

            if (navParams is PageNavigationParams<ResultNavParams> results)
            {
                Result = results.Payload.Result;
                Message = results.Payload.Message;

                IconUnicode = Result ? "\uf00c" : "\uf00d";
            }
        }
    }
}

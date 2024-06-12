using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Mvvm;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Contract.Helper
{
    public class FastingDataMessageProxy : BindableBase
    {
        private readonly string _text;

        private IEnumerable<FastingDataProxy> _data;
        private string _message;
        private bool _showMessage;

        public FastingDataMessageProxy(IEnumerable<FastingDataProxy> proxies, string text, DateTime timeToUse)
        {
            _text = text;

            Data = proxies;
            ShowMessage = _data != null && _data.Any();

            if (!ShowMessage) return;

            Message = SetFastingMessage(timeToUse);
        }

        public IEnumerable<FastingDataProxy> Data
        {
            get => _data;
            private set => SetProperty(ref _data, value);
        }

        public bool ShowMessage
        {
            get => _showMessage;
            private set => SetProperty(ref _showMessage, value);
        }

        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }

        private string SetFastingMessage(DateTime dateTimeInUse)
        {
            var sb = new StringBuilder();
            sb.AppendLine(_text);
            foreach (var proxy in Data)
            {
                string from;
                if (proxy.StartDateTime.Day < dateTimeInUse.Day)
                {
                    var dateTimeReset = new DateTime(dateTimeInUse.Year, dateTimeInUse.Month, dateTimeInUse.Day);
                    from = DateTimeHelper.GetTimeFormat(dateTimeReset);
                }
                else
                {
                    from = DateTimeHelper.GetTimeFormat(proxy.StartDateTime);
                }

                string to;
                if (proxy.FinishDateTime.Day > dateTimeInUse.Day)
                {
                    var dateTimeReset = new DateTime(dateTimeInUse.Year, dateTimeInUse.Month, dateTimeInUse.Day, 23, 59,
                        59);
                    to = DateTimeHelper.GetTimeFormat(dateTimeReset);
                }
                else
                {
                    to = DateTimeHelper.GetTimeFormat(proxy.FinishDateTime);
                }

                if (proxy.FinishDateTime >= DateTime.Now)
                {
                    sb.AppendLine($"{from} - {to}");
                }
                else
                    sb.Clear();
                
            }


            return sb.ToString();
        }
    }

    public class EmptyFastingDataMessageProxy : FastingDataMessageProxy
    {
        public EmptyFastingDataMessageProxy() : base(null, string.Empty, DateTime.Now)
        {
            
        }
    }
}
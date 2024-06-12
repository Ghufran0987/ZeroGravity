using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class AnswerOptionProxy : ProxyBase
    {
        public AnswerOptionProxy(QuestionProxy question)
        {
            _question = question;
        }

        private string _value;
        private string _displayText;
        private QuestionProxy _question;
        private string _tag;

        public QuestionProxy Question
        {
            get => _question;
        }

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public string Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }

        public string DisplayText
        {
            get => _displayText;
            set => SetProperty(ref _displayText, value);
        }
    }
}
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class QuestionAnswersProxy : ProxyBase
    {
        private int _accountId;
        private int _questionId;
        private int _answerId;
        private string _value;
        private string _tag1;
        private string _tag2;
        private string _tag3;
        private int _personDataId;

        public QuestionAnswersProxy()
        {
        }

        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }

        public int PersonDataId
        { 
            get => _personDataId;
            set => SetProperty(ref _personDataId, value);
        }

        public int QuestionId
        {
            get => _questionId;
            set => SetProperty(ref _questionId, value);
        }

        public int AnswerId
        {
            get => _answerId;
            set => SetProperty(ref _answerId, value);
        }


        public string Tag1
        {
            get => _tag1;
            set => SetProperty(ref _tag1, value);
        }
        public string Tag2
        {
            get => _tag2;
            set => SetProperty(ref _tag2, value);
        }

        public string Tag3
        {
            get => _tag3;
            set => SetProperty(ref _tag3, value);
        }

        public string Value
        {
            get => _value ;
            set => SetProperty(ref _value , value);
        }
    }

}
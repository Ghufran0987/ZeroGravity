using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class QuestionProxy : ProxyBase
    {
        public QuestionProxy()
        {
            _answers = new ObservableCollection<AnswerOptionProxy>();
            _selectedAnswers = new ObservableCollection<object>();
            _tagOptions = new ObservableCollection<string>();
        }

        private string _name;
        private double _number;
        private bool _isActive;
        private string _category;
        private string _imageUrl;
        private string _backgroundImageUrl;
        private string _discription;
        private string _subtitle;
        private string _tag1;
        private string _tag2;
        private string _tag3;
        private QuestionType _type;
        private QuestionTemplate _template;
        private ObservableCollection<AnswerOptionProxy> _answers;
        private ObservableCollection<object> _selectedAnswers;
        private ObservableCollection<string> _tagOptions;

        private AnswerOptionProxy _selectedValue;
        private string _selectedTag;
        private AnswerDataType _answerDataType;
        private DataFieldType _dataFieldType;
        private bool _showInOnbaording;
        private string _textColor = "#FFF";

        public AnswerOptionProxy SelectedAnswer
        {
            get
            {
                if (_selectedValue == null)
                {
                    if (_answers != null && _answers?.Count > 0)
                    {
                        if (Type == QuestionType.ManualEntry)
                        {
                            _selectedValue = _answers[0];
                        }
                    }
                }
                return _selectedValue;
            }
            set => SetProperty(ref _selectedValue, value);
        }

        public string SelectedTag
        {
            get => _selectedTag;
            set => SetProperty(ref _selectedTag, value);
        }

        public ObservableCollection<string> TagOptions
        {
            get => _tagOptions;
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public string BackgroundImageUrl
        {
            get => _backgroundImageUrl;
            set => SetProperty(ref _backgroundImageUrl, value);
        }

        public string Discription
        {
            get => _discription;
            set => SetProperty(ref _discription, value);
        }

        public string Subtitle
        {
            get => _subtitle;
            set => SetProperty(ref _subtitle, value);
        }

        public string Tag1
        {
            get => _tag1;
            set
            {
                SetProperty(ref _tag1, value);
                if (!string.IsNullOrEmpty(_tag1))
                {
                    TagOptions.Add(_tag1);
                }
            }
        }

        public string Tag2
        {
            get => _tag2;
            set
            {
                SetProperty(ref _tag2, value);
                if (!string.IsNullOrEmpty(_tag2))
                {
                    TagOptions.Add(_tag2);
                }
            }
        }

        public string Tag3
        {
            get => _tag3;
            set
            {
                SetProperty(ref _tag3, value);
                if (!string.IsNullOrEmpty(_tag3))
                {
                    TagOptions.Add(_tag3);
                }
            }
        }

        public QuestionType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public QuestionTemplate Template
        {
            get => _template;
            set => SetProperty(ref _template, value);
        }

        public ObservableCollection<AnswerOptionProxy> Answers
        {
            get => _answers;
        }

        public ObservableCollection<object> SelectedAnswers
        {
            get => _selectedAnswers;
            set => SetProperty(ref _selectedAnswers, value);
        }

        public AnswerDataType AnswerDataType
        {
            get => _answerDataType;
            set => SetProperty(ref _answerDataType, value);
        }

        public DataFieldType DataFieldType
        {
            get => _dataFieldType;
            set => SetProperty(ref _dataFieldType, value);
        }

        public bool ShowInOnbaording
        {
            get => _showInOnbaording;
            set => SetProperty(ref _showInOnbaording, value);
        }

        public string TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private ImageSource _backgroundImageSource;

        public ImageSource BackgroundImageSource
        {
            get => _backgroundImageSource;
            set => SetProperty(ref _backgroundImageSource, value);
        }

        private ImageSource _imageSource;

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }
    }
}
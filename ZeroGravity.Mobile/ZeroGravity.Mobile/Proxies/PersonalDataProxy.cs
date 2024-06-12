using System.Collections.Generic;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class PersonalDataProxy : ProxyBase
    {
        private readonly IAlgorithmService _algorithmService;

        private double _bodyFat = 0;

        private double _bodyMassIndex = 0;

        private string _country = string.Empty;

        private int _dateTimeDisplayType = 0;

        private string _firstName = string.Empty;

        private double _height = 50;

        private double _hipDiameter = 50;

        private string _lastName = string.Empty;

        private double _neckDiameter = 20;

        private SalutationType _salutation = SalutationType.Mr;

        private int _unitDisplayType = 0;

        private double _waistDiameter = 50;

        private double _weight = 40;

        private GenderBiologicalType _genderType = GenderBiologicalType.Undefined;

        private int _yearOfBirth = 1930;
        private string _ethnicity = string.Empty;

        private List<QuestionAnswersProxy> _questionAnswers;
        private string _timezoneId;

        public PersonalDataProxy()
        {
            _questionAnswers = new List<QuestionAnswersProxy>();
            _algorithmService = new AlgorithmService();
        }

        public int AccountId { get; set; }

        public DeviceType DeviceType { get; set; }

        public SalutationType Salutation
        {
            get => _salutation;
            set
            {
                SetProperty(ref _salutation, value);
            }
        }

        public GenderBiologicalType Gender
        {
            get => _genderType;
            set
            {
                if (SetProperty(ref _genderType, value))
                    BodyFat = _algorithmService.CalculateBodyFatPercentage(_genderType, WaistDiameter,
                        HipDiameter, NeckDiameter, Height);
            }
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }

        public int YearOfBirth
        {
            get => _yearOfBirth;
            set => SetProperty(ref _yearOfBirth, value);
        }

        public double Weight
        {
            get => _weight;
            set
            {
                if (SetProperty(ref _weight, value)) BodyMassIndex = _algorithmService.CalculateBmi(Height, Weight);
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                if (SetProperty(ref _height, value))
                {
                    BodyMassIndex = _algorithmService.CalculateBmi(Height, Weight);
                    BodyFat = _algorithmService.CalculateBodyFatPercentage(Gender, WaistDiameter,
                        HipDiameter, NeckDiameter, Height);
                }
            }
        }

        public double WaistDiameter
        {
            get => _waistDiameter;
            set
            {
                if (SetProperty(ref _waistDiameter, value))
                    BodyFat = _algorithmService.CalculateBodyFatPercentage(Gender, WaistDiameter,
                        HipDiameter, NeckDiameter, Height);
            }
        }

        public double HipDiameter
        {
            get => _hipDiameter;
            set
            {
                if (SetProperty(ref _hipDiameter, value))
                    BodyFat = _algorithmService.CalculateBodyFatPercentage(Gender, WaistDiameter,
                        HipDiameter, NeckDiameter, Height);
            }
        }

        public double NeckDiameter
        {
            get => _neckDiameter;
            set
            {
                if (SetProperty(ref _neckDiameter, value))
                    BodyFat = _algorithmService.CalculateBodyFatPercentage(Gender, WaistDiameter,
                        HipDiameter, NeckDiameter, Height);
            }
        }

        public double BodyMassIndex
        {
            get => _bodyMassIndex;
            set => SetProperty(ref _bodyMassIndex, value);
        }

        public double BodyFat
        {
            get => double.IsNaN(_bodyFat) ? 0 : _bodyFat;
            set => SetProperty(ref _bodyFat, value);
        }

        public int UnitDisplayType
        {
            get => _unitDisplayType;
            set => SetProperty(ref _unitDisplayType, value);
        }

        public int DateTimeDisplayType
        {
            get => _dateTimeDisplayType;
            set => SetProperty(ref _dateTimeDisplayType, value);
        }

        public string Ethnicity
        {
            get => _ethnicity;
            set => SetProperty(ref _ethnicity, value);
        }

        public List<QuestionAnswersProxy> QuestionAnswers
        {
            get => _questionAnswers;
            set => SetProperty(ref _questionAnswers, value);
        }

        public string TimezoneId
        {
            get => _timezoneId;
            set => SetProperty(ref _timezoneId, value);
        }
    }
}
using Prism.Mvvm;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class MedicalPreconditionsProxy : BindableBase
    {
        private DiabetesType _diabetesType;

        private bool _hasArthritis;

        private bool _hasCardiacCondition;

        private bool _hasDiabetes;

        private bool _hasHypertension;
        private bool _dontWantToSayNow;
        private bool _others;

        public int Id { get; set; }

        public int AccountId { get; set; }

        public bool HasDiabetes
        {
            get => _hasDiabetes;
            set => SetProperty(ref _hasDiabetes, value);
        }

        public DiabetesType DiabetesType
        {
            get => _diabetesType;
            set => SetProperty(ref _diabetesType, value);
        }

        public bool HasHypertension
        {
            get => _hasHypertension;
            set => SetProperty(ref _hasHypertension, value);
        }

        public bool HasArthritis
        {
            get => _hasArthritis;
            set => SetProperty(ref _hasArthritis, value);
        }

        public bool HasCardiacCondition
        {
            get => _hasCardiacCondition;
            set => SetProperty(ref _hasCardiacCondition, value);
        }
    
        public bool DontWantToSayNow
        {
            get => _dontWantToSayNow;
            set => SetProperty(ref _dontWantToSayNow, value);
        }

        public bool Others
        {
            get => _others;
            set => SetProperty(ref _others, value);
        }
       
    }
}
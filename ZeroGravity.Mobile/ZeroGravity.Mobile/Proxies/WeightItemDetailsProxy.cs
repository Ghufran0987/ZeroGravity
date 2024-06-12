using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZeroGravity.Mobile.Services;

namespace ZeroGravity.Mobile.Proxies
{
    public class WeightItemDetailsProxy : BindableBase
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        private DateTime _created;

        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        private DateTime? _completed;

        public DateTime? Completed
        {
            get { return _completed; }
            set { _completed = value; }
        }

        private double _initialWeight;

        public double InitialWeight
        {
            get { return _initialWeight; }
            set { SetProperty(ref _initialWeight, value); }
        }

        private double _targetWeight;

        public double TargetWeight
        {
            get { return _targetWeight; }
            set { _targetWeight = value; }
        }

        private double _currentWeight;

        public double CurrentWeight
        {
            get { return _currentWeight; }
            set { _currentWeight = value; }
        }


        private ObservableCollection<WeightItemProxy> _weightItemProxies;

        public ObservableCollection<WeightItemProxy> WeightItemProxies
        {
            get { return _weightItemProxies; }
            set { SetProperty(ref _weightItemProxies, value); }
        }

        public WeightItemDetailsProxy()
        {
            WeightItemProxies = new ObservableCollection<WeightItemProxy>();
            DispalyUnit();
        }


        private string _displayUnit;

        public string DisplayUnit
        {
            get { return _displayUnit; }
            set { _displayUnit = value; }
        }


        private void DispalyUnit()
        {
            var dis = DisplayConversionService.GetDisplayPrefences();
            
            switch (dis.UnitDisplayType)
            {
                case Shared.Enums.UnitDisplayType.Imperial:
                    DisplayUnit = "Pound";
                    break;
                case Shared.Enums.UnitDisplayType.Metric:
                    DisplayUnit = " Kg";
                    break;
                default:
                    DisplayUnit = " Kg";
                    break;
            }

          
        }
    }
}

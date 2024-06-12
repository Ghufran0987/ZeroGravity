using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Syncfusion.SfRangeSlider.XForms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.ViewModels
{
    public class DemoPageViewModel : VmBase<IDemoPage, IDemoPageVmProvider, DemoPageViewModel>
    {
        private IEnumerable<string> _comboBoxDataSource;
        private double _minDuration;
        private double _maxDuration;
        private double _duration;
        private double _minWeight;
        private double _maxWeight;
        private double _weight;
        private ObservableCollection<Items> _customLabelsCollection;
        private bool _timePickerIsOpen;
        private TimeSpan _time;
        private IEnumerable<ZgButtonGroupItem> _groupItems;
        private ZgButtonGroupItem _selectedGroupItem;

        public DemoPageViewModel(IVmCommonService service, IDemoPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            Initialize();
        }

        public IEnumerable<string> ComboBoxDataSource
        {
            get => _comboBoxDataSource;
            set => SetProperty(ref _comboBoxDataSource, value);
        }

        public double MinDuration
        {
            get => _minDuration;
            set => SetProperty(ref _minDuration, value);
        }

        public double MaxDuration
        {
            get => _maxDuration;
            set => SetProperty(ref _maxDuration, value);
        }

        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public double MinWeight
        {
            get => _minWeight;
            set => SetProperty(ref _minWeight, value);
        }

        public double MaxWeight
        {
            get => _maxWeight;
            set => SetProperty(ref _maxWeight, value);
        }

        public double Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        public ObservableCollection<Items> CustomLabelsCollection
        {
            get => _customLabelsCollection;
            set => SetProperty(ref _customLabelsCollection, value);
        }

        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public bool TimePickerIsOpen
        {
            get => _timePickerIsOpen;
            set => SetProperty(ref _timePickerIsOpen, value);
        }

        public IEnumerable<ZgButtonGroupItem> GroupItems
        {
            get => _groupItems;
            set => SetProperty(ref _groupItems, value);
        }

        public ZgButtonGroupItem SelectedGroupItem
        {
            get => _selectedGroupItem;
            set => SetProperty(ref _selectedGroupItem, value);
        }

        public DelegateCommand ShowTimePickerCommand { get; private set; }

        private void Initialize()
        {
            InitRangeSliderProperties();
            InitComboBoxDataSource();

            ShowTimePickerCommand = new DelegateCommand(() =>
            {
                TimePickerIsOpen = !TimePickerIsOpen;
            });
        }

        private void InitRangeSliderProperties()
        {
            MinDuration = 0;
            MaxDuration = 10;
            Duration = 3;

            MinWeight = 0;
            MaxWeight = 320;
            Weight = 80;

            CustomLabelsCollection = new ObservableCollection<Items>
            {
                new Items { Value = ActivityConstants.DayToDayDurationThreshold, Label = "\u25B3" }
            };

            var groupItemA = new ZgButtonGroupItem() {Label = "Manual"};
            var groupItemB = new ZgButtonGroupItem() {Label = "Sync"};
            GroupItems = new List<ZgButtonGroupItem>
            {
                groupItemA,
                groupItemB
            };

            SelectedGroupItem = groupItemB;
        }

        private void InitComboBoxDataSource()
        {
            var items = new List<string>();

            for (var i = 1; i < 11; i++)
            {
                items.Add($"Item {i}");
            }

            ComboBoxDataSource = items;
        }
    }
}

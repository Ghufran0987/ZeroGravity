using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Proxies
{
    public class SyncActivityProxy : ExerciseActivityProxy
    {
        private string _description;

        private ObservableCollection<ComboBoxItem> _intensitySource;

        private bool _isSelectedForSync;

        private string _name;


        private int _selectedIntensity;

        public SyncActivityProxy()
        {
            CreateIntensitySource();
        }

        public bool IsSelectedForSync
        {
            get => _isSelectedForSync;
            set => SetProperty(ref _isSelectedForSync, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public int SelectedIntensity
        {
            get => _selectedIntensity;
            set => SetProperty(ref _selectedIntensity, value);
        }

        public ObservableCollection<ComboBoxItem> IntensitySource
        {
            get => _intensitySource;
            set => SetProperty(ref _intensitySource, value);
        }

        private void CreateIntensitySource()
        {
            var comboBoxItems = new List<ComboBoxItem>
            {
                new ComboBoxItem {Id = 0, Text = AppResources.Activity_Intensity_Low},
                new ComboBoxItem {Id = 1, Text = AppResources.Activity_Intensity_Moderate},
                new ComboBoxItem {Id = 2, Text = AppResources.Activity_Intensity_Vigorous}
            };

            IntensitySource = new ObservableCollection<ComboBoxItem>(comboBoxItems);
        }
    }
}
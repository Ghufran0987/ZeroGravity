using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class WeightMilestoneProxy : BindableBase
    {
        public int MileStoneNo { get; set; }

        public double MileStoneTargetWeight { get; set; }

        public double MileStoneStartingWeight { get; set; }


        private MilestoneStatus _status;

        public MilestoneStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private bool _isMileStoneCompletedVisible;

        public bool IsMileStoneCompletedVisible
        {
            get { return _isMileStoneCompletedVisible; }
            set { SetProperty(ref _isMileStoneCompletedVisible, value); }
        }

        private bool _isMileStoneInCompletedVisible;

        public bool IsMileStoneInCompletedVisible
        {
            get { return _isMileStoneInCompletedVisible; }
            set { SetProperty(ref _isMileStoneInCompletedVisible, value); }
        }

        public string MileStone { get { return "Milestone " + MileStoneNo; } }

        private string _milestoneCongMesg;

        public string MilestoneCongMesg
        {
            get { return _milestoneCongMesg; }
            set { SetProperty(ref _milestoneCongMesg, value); }
        }

        private string _milestoneWeightMesg;

        public string MilestoneWeightMesg
        {
            get { return _milestoneWeightMesg; }
            set { SetProperty(ref _milestoneWeightMesg, value); }
        }

        private Color _milestoneBackground;

        public Color MilestoneBackground
        {
            get { return _milestoneBackground; }
            set { SetProperty(ref _milestoneBackground, value); }
        }

        private ImageSource _mileStoneImageSource;

        public ImageSource MileStoneImageSource
        {
            get => _mileStoneImageSource;
            set => SetProperty(ref _mileStoneImageSource, value);
        }

        private GridLength _mileStoneHeight;

        public GridLength MileStoneHeight
        {
            get => _mileStoneHeight;
            set => SetProperty(ref _mileStoneHeight, value);
        }

        private Thickness _sideBorderThickness;

        public Thickness SideBorderThickness
        {
            get => _sideBorderThickness;
            set => SetProperty(ref _sideBorderThickness, value);
        }

    }
}

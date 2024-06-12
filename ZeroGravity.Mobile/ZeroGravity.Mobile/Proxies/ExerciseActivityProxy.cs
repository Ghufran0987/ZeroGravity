using System;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class ExerciseActivityProxy : ActivityProxyBase
    {
        private DateTime _exerciseDateTime;
        private TimeSpan _exerciseTime;
        private int _exerciseType;
        private string _name;
        private int _selectedIntensityIndex;

        public ExerciseActivityProxy()
        {
            DailyThreshold = ActivityConstants.ExerciseDurationThreshold;
        }

        public int SelectedIntensityIndex
        {
            get => _selectedIntensityIndex;
            set
            {
                if (SetProperty(ref _selectedIntensityIndex, value))
                {
                    ActivityIntensityType = (ActivityIntensityType) _selectedIntensityIndex;
                }
            }
        }

        public ActivityIntensityType ActivityIntensityType { get; set; }

        public int AccountId { get; set; }

        public int ExerciseType
        {
            get => _exerciseType;
            set => SetProperty(ref _exerciseType, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        public ActivityType ActivityType => ActivityType.Exercise;

        public TimeSpan ExerciseTime
        {
            get => _exerciseTime;
            set => SetProperty(ref _exerciseTime, value);
        }

        public DateTime ExerciseDateTime
        {
            get => _exerciseDateTime;
            set => SetProperty(ref _exerciseDateTime, value);
        }
    }
}
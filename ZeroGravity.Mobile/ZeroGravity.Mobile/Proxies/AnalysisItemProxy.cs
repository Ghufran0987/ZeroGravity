using System;
using System.Collections.Generic;
using Prism.Mvvm;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using System.Linq;

namespace ZeroGravity.Mobile.Proxies
{
    public class AnalysisItemProxy : BindableBase
    {
        private string _description;
        private double _goal;
        private double _goalMax;
        private string _iconString;
        private bool _isHealthy;
        private bool _showCircularProgress;
        private bool _showEllipseProgress;
        private bool _showWaterCupProgress;
        private bool _isOnBoarding;

        private double _circularProgressCount;
        private double _maximum;
        private double _sum;
        private double _sumMax;

        private string _subTitleLabel;
        private string _fillColor;
        private string _fillText;

        private bool _showCalorieCupProgress;

        public AnalysisItemProxy(string iconString, string description, bool isHealthy, double goal, double goalMax, double sum,
            double sumMax, string fillText, bool isOnboarding)
        {
            IsOnBoarding = isOnboarding;
            IconString = iconString;
            Description = description;
            IsHealthy = isHealthy;
            Goal = goal;
            Sum = sum;
            GoalMax = goalMax;
            SumMax = sumMax;
            var fillValue = isOnboarding ? SumMax : Goal;
            FillText = GetFillText(fillValue);
            FillColor = GetFillColor(fillText);
            SubTitleLabel = GetSubTitle();
            SubTitleGoalLabel = GetSubTitleGoalLabel();
            CircularProgressCount = isOnboarding ? 100 : (Goal * 100 / SumMax);
            CupCount = Enumerable.Range(1, (int)GoalMax).Select(X => (int)X).ToList();
        }

        private string GetSubTitleGoalLabel()
        {
            if (Description == AppResources.AnalysisPage_Items_Water)
            {
                var fillValue = IsOnBoarding ? Goal : SumMax;
                var subtitle = string.Format("{0}{1} cups per day", "Your target is ", fillValue);
                FillColor = "#33E1FF";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Activity)
            {
                var fillValue = IsOnBoarding ? Goal : SumMax;

                var subtitle = string.Format("{0}{1} hrs", "Your goal was ", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_CalorieDrink)
            {
                var fillValue = IsOnBoarding ? Goal : SumMax;
                var subtitle = string.Format("{0}{1} cups per day", "Your target is ", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Breakfast)
            {
                var subtitle = string.Format("{0}{1} breakfast", "Your target is a ", FillText);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Dinner)
            {
                var subtitle = string.Format("{0}{1} dinner", "Your target is a ", FillText);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Lunch)
            {
                var subtitle = string.Format("{0}{1} lunch", "Your target is a ", FillText);
                return subtitle;
            }
            return string.Format("{0}{1}", "Your goal was ", FillText);
        }

        private string GetSubTitle()
        {
            if (Description == AppResources.AnalysisPage_Items_CalorieDrink)
            {
                if (Goal <= Sum)
                    FillColor = "#FF5F58";
                else
                    FillColor = "#FA9917";
            }
            if (_isHealthy)
                return "Goal Achieved";
            else if (Description == AppResources.AnalysisPage_Items_Water)
            {
                var fillValue = IsOnBoarding ? Goal : SumMax;
                var subtitle = string.Format("{0}{1} cups", "Your goal was ", fillValue);
                FillColor = "#33E1FF";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Activity)
            {
                var fillValue = IsOnBoarding ? Goal : SumMax;

                var subtitle = string.Format("{0}{1} hrs", "Your goal was ", fillValue);
                FillText = string.Format("{0}{1}", Goal, " hrs");
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_CalorieDrink)
            {
                var fillValue = IsOnBoarding ? Goal : SumMax;
                var subtitle = string.Format("{0}{1} cups", "Your goal was ", fillValue);
                return subtitle;
            }

            return string.Format("{0}{1}", "Your goal was ", FillText);
        }

        private string GetFillText(double sumMax)
        {
            List<string> filterList = new List<string>() { AppResources.AnalysisPage_Items_Breakfast,
                AppResources.AnalysisPage_Items_Dinner,
                AppResources.AnalysisPage_Items_HealthySnack,
                AppResources.AnalysisPage_Items_Lunch,
                AppResources.AnalysisPage_Items_UnhealthySnack,
            };
            if (filterList.Contains(Description))
            {
                if (sumMax > 5) sumMax = 5;
                FoodAmountType foodAmountType = (FoodAmountType)Enum.ToObject(typeof(FoodAmountType), Convert.ToInt32(sumMax));

                string foodAmountString = string.Empty;
                switch (foodAmountType)
                {
                    case FoodAmountType.VeryLight:
                        foodAmountString = AppResources.FoodAmount_VeryLight;
                        break;

                    case FoodAmountType.Light:
                        foodAmountString = AppResources.FoodAmount_Light;
                        break;

                    case FoodAmountType.Medium:
                        foodAmountString = AppResources.FoodAmount_Medium;
                        break;

                    case FoodAmountType.Heavy:
                        foodAmountString = AppResources.FoodAmount_Heavy;
                        break;

                    case FoodAmountType.VeryHeavy:
                        foodAmountString = AppResources.FoodAmount_VeryHeavy;
                        break;

                    case FoodAmountType.Undefined:
                        foodAmountString = AppResources.FoodAmount_NotAvailable;
                        break;
                }
                return foodAmountString;
            }

            return sumMax.ToString();
        }

        private string GetFillColor(string fillText)
        {
            string fillColorValue = string.Empty;
            switch (fillText)
            {
                case "VeryLight": fillColorValue = "#52a89a"; break;
                case "Light": fillColorValue = "#319C8A"; break;
                case "Medium": fillColorValue = "#FA9917"; break;
                case "Heavy": fillColorValue = "#FF5869"; break;
                case "VeryHeavy": fillColorValue = "#FF5F58"; break;
                default: break;
            }
            return fillColorValue;
        }

        public string SubTitleLabel
        {
            get => _subTitleLabel;
            set => SetProperty(ref _subTitleLabel, value);
        }

        public string FillColor
        {
            get => _fillColor;
            set => SetProperty(ref _fillColor, value);
        }

        public string FillText
        {
            get => _fillText;
            set => SetProperty(ref _fillText, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public double GoalMax
        {
            get => _goalMax;
            set => SetProperty(ref _goalMax, value);
        }

        public double SumMax
        {
            get => _sumMax;
            set => SetProperty(ref _sumMax, value);
        }

        public string IconString
        {
            get => _iconString;
            set => SetProperty(ref _iconString, value);
        }

        public bool IsHealthy
        {
            get => _isHealthy;
            set => SetProperty(ref _isHealthy, value);
        }

        public bool IsOnBoarding
        {
            get => _isOnBoarding;
            set => SetProperty(ref _isOnBoarding, value);
        }

        public double Goal
        {
            get => _goal;
            set
            {
                if (value == 0)
                {
                    // offset dazu addieren, damit auch bei 0 / None ein Balken angezeigt wird
                    SetProperty(ref _goal, value + 0.025);
                }
                else
                {
                    SetProperty(ref _goal, value);
                }
            }
        }

        public bool ShowCircularProgress
        {
            get => _showCircularProgress;
            set => SetProperty(ref _showCircularProgress, value);
        }

        public bool ShowWaterCupProgress
        {
            get => _showWaterCupProgress;
            set => SetProperty(ref _showWaterCupProgress, value);
        }

        public bool ShowEllipseProgress
        {
            get => _showEllipseProgress;
            set => SetProperty(ref _showEllipseProgress, value);
        }

        public bool ShowCalorieCupProgress
        {
            get { return _showCalorieCupProgress; }
            set { _showCalorieCupProgress = value; }
        }

        public double CircularProgressCount
        {
            get => _circularProgressCount;
            set => SetProperty(ref _circularProgressCount, value);
        }

        public double Sum
        {
            get => _sum;
            set => SetProperty(ref _sum, value);
        }

        private List<int> _cupCount;

        public List<int> CupCount
        {
            get => _cupCount;
            set => SetProperty(ref _cupCount, value);
        }

        private string _subTitleGoalLabel;

        public string SubTitleGoalLabel
        {
            get { return _subTitleGoalLabel; }
            set { _subTitleGoalLabel = value; }
        }
    }
}
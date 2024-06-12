using System;
using System.Collections.Generic;
using Prism.Mvvm;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using System.Linq;

namespace ZeroGravity.Mobile.Proxies
{
    public class PersonalGoalItemProxy : BindableBase
    {
        private string _description;
        private double _goal;
        private double _goalMax;
        private string _iconString;
        private bool _showCircularProgress;
        private bool _showEllipseProgress;
        private bool _showWaterCupProgress;
        private double _circularProgressCount;
        private double _sumMax;

        private string _subTitleLabel;
        private string _fillColor;
        private string _fillText;

        private bool _showCalorieCupProgress;

        public PersonalGoalItemProxy(string iconString, string description, double goal, string fillText)
        {
            IconString = iconString;
            Description = description;
            Goal = goal;
            FillText = GetFillText(Goal);
            FillColor = GetFillColor(fillText);
            SubTitleLabel = GetSubTitle();
            SubTitleGoalLabel = GetSubTitleGoalLabel();
            CircularProgressCount = 100;
            CupCount = Enumerable.Range(1, (int)(Goal / 0.25)).Select(X => X).ToList();
        }

        private string GetSubTitleGoalLabel()
        {
            if (Description == AppResources.AnalysisPage_Items_Water)
            {
                var fillValue = (int)(Goal / 0.25);
                var subtitle = string.Format("Your target is {0} Cups", fillValue);
                FillColor = "#0497FF";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Activity)
            {
                var fillValue = Goal;
                var subtitle = string.Format("Your target is {0} hrs", fillValue);
                FillText = string.Format("{0} hrs", Goal);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_CalorieDrink)
            {
                var fillValue = (int)(Goal / 0.25);
                var subtitle = string.Format("Your target is {0} Cups", fillValue);
                FillColor = "#0497FF";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Breakfast)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Breakfast", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Lunch)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Lunch", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Dinner)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Dinner", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_HealthySnack)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Healthy Snacks", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_UnhealthySnack)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Unhealthy Snacks", fillValue);
                return subtitle;
            }
            return string.Format("{0}{1}", "Your goal was ", FillText);
        }

        private string GetSubTitle()
        {
            if (Description == AppResources.AnalysisPage_Items_Water)
            {
                var fillValue = (int)(Goal / 0.25);
                var subtitle = string.Format("Your target is {0} Cups", fillValue);
                FillColor = "#33E1FF";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Activity)
            {
                var fillValue = Goal;
                var subtitle = string.Format("Your target is {0} hrs", fillValue);
                FillText = string.Format("{0} hrs", Goal);
                FillColor = "#4F4F4F";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_CalorieDrink)
            {
                var fillValue = (int)(Goal / 0.25);
                var subtitle = string.Format("Your target is {0} Cups", fillValue);
                FillColor = "#4F4F4F";
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Breakfast)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Breakfast", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Lunch)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Lunch", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_Dinner)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Dinner", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_HealthySnack)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Healthy Snacks", fillValue);
                return subtitle;
            }
            else if (Description == AppResources.AnalysisPage_Items_UnhealthySnack)
            {
                var fillValue = (FoodAmountType)Goal;
                var subtitle = string.Format("Your target is a {0} Unhealthy Snacks", fillValue);
                return subtitle;
            }

            return string.Format("Your target is {0}", FillText);
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
                var max = Goal > 5 ? 5 : Goal;
                FoodAmountType foodAmountType = (FoodAmountType)Enum.ToObject(typeof(FoodAmountType), Convert.ToInt32(max));

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

        public double Goal
        {
            get => _goal;
            set
            {
                SetProperty(ref _goal, value);
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
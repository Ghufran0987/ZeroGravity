using System;
using System.Collections.Generic;
using Prism.Mvvm;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using System.Linq;
using ZeroGravity.Mobile.Resources.Fonts;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace ZeroGravity.Mobile.Proxies
{
    public class ProgressProxy : BindableBase
    {
        private LiquidProgress _waterProgress;

        public LiquidProgress WaterProgress
        {
            get => _waterProgress;
            set => SetProperty(ref _waterProgress, value);
        }

        private int _totalScore;

        public int TotalScore
        {
            get => _totalScore;
            set => SetProperty(ref _totalScore, value);
        }

        private LiquidProgress _calorieProgress;

        public LiquidProgress CalorieProgress
        {
            get => _calorieProgress;
            set => SetProperty(ref _calorieProgress, value);
        }

        private FoodProgress _breakFastProgress;

        public FoodProgress BreakFastProgress
        {
            get => _breakFastProgress;
            set => SetProperty(ref _breakFastProgress, value);
        }

        private FoodProgress _lunchProgress;

        public FoodProgress LunchProgress
        {
            get => _lunchProgress;
            set => SetProperty(ref _lunchProgress, value);
        }

        private FoodProgress _dinnerProgress;

        public FoodProgress DinnerProgress
        {
            get => _dinnerProgress;
            set => SetProperty(ref _dinnerProgress, value);
        }

        private FoodProgress _healthySnacksProgress;

        public FoodProgress HealthySnacksProgress
        {
            get => _healthySnacksProgress;
            set => SetProperty(ref _healthySnacksProgress, value);
        }

        private FoodProgress _unHealthySnacksProgress;

        public FoodProgress UnHealthySnacksProgress
        {
            get => _unHealthySnacksProgress;
            set => SetProperty(ref _unHealthySnacksProgress, value);
        }

        private FoodProgress _activitiesProgress;

        public FoodProgress ActivitiesProgress
        {
            get => _activitiesProgress;
            set => SetProperty(ref _activitiesProgress, value);
        }

        private FoodProgress _fastingProgress;

        public FoodProgress FastingProgress
        {
            get => _fastingProgress;
            set => SetProperty(ref _fastingProgress, value);
        }

        private FoodProgress _meditationProgress;

        public FoodProgress MeditationProgress
        {
            get => _meditationProgress;
            set => SetProperty(ref _meditationProgress, value);
        }

        private MetabolicProgress _metaScore;
        public MetabolicProgress MetaScore
        {
            get => _metaScore;
            set => SetProperty(ref _metaScore, value);
        }

        public int GetTotalScore(bool isMetascoreDeviceActive)
        {
            int retScore = (WaterProgress.Score
               + this.UnHealthySnacksProgress.Score
               + this.ActivitiesProgress.Score
               + this.BreakFastProgress.Score
               + this.CalorieProgress.Score
               + this.DinnerProgress.Score
               + this.FastingProgress.Score
               + this.HealthySnacksProgress.Score
               + this.LunchProgress.Score
               + this.MeditationProgress.Score);


            if (isMetascoreDeviceActive)
            {
                retScore = retScore + this.MetaScore.Score;
            }

            if (retScore > 0)
            {
                if (isMetascoreDeviceActive)
                {
                    retScore = retScore / 11;
                }
                else
                {
                    retScore = retScore / 10;
                }
            }
            else
            {
                retScore = 0;
            }
            TotalScore = retScore;

            return retScore;
        }

        public void GetMessages()
        {
            if (this.TotalScore > 75)
            {
                Message = "Brilliant";
            }
            else
            {
                Message = "You need to work on this.";
            }

            if (this.WaterProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Water", "You're doing Great. Keep it up. ");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Water", "You need to work on this. ");
                Improvement.Add(item);
            }

            if (this.BreakFastProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Breakfast", "Keep up the small protion sizes.");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Breakfast", "Try gradually reducing your portion sizes");
                Improvement.Add(item);
            }

            if (this.LunchProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Lunch", "You're doing Great. Moderation is the key!");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Lunch", "Try gradually reducing your portion sizes");
                Improvement.Add(item);
            }

            if (this.DinnerProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Dinner", "Keep up the small protion sizes.");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Dinner", "Try gradually reducing your portion sizes");
                Improvement.Add(item);
            }
            if (this.HealthySnacksProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Healthy Snacks", "You're doing Great. Keep it up. ");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Healthy Snacks", "You need to improve on this.");
                Improvement.Add(item);
            }

            if (this.UnHealthySnacksProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Unhealthy Snacks", "You're doing Great. Keep it up. ");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Unhealthy Snacks", "Rid yourself of all sweets and snacks ");
                Improvement.Add(item);
            }

            if (this.CalorieProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Calorie Drinks", "You are doing great. Keep it up. ");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Calorie Drinks", "Go easy on these");
                Improvement.Add(item);
            }

            if (this.FastingProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Fasting", "You're doing Great. Keep it up. ");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Fasting", "You need to improve on this. ");
                Improvement.Add(item);
            }

            if (this.MeditationProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Meditation", "You're taking out time to relax and wind down.");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Meditation", "You need to improve on this. ");
                Improvement.Add(item);
            }

            if (this.ActivitiesProgress.Score >= 75)
            {
                var item = new KeyValuePair<string, string>("Activities", "You are on track.");
                Right.Add(item);
            }
            else
            {
                var item = new KeyValuePair<string, string>("Activities", "You need to work on this. ");
                Improvement.Add(item);
            }
        }

        private ObservableCollection<KeyValuePair<string, string>> _right;

        public ObservableCollection<KeyValuePair<string, string>> Right
        {
            get
            {
                if (_right == null)
                {
                    _right = new ObservableCollection<KeyValuePair<string, string>>();
                }
                return _right;
            }

            set => SetProperty(ref _right, value);
        }

        private ObservableCollection<KeyValuePair<string, string>> _improvement;

        public ObservableCollection<KeyValuePair<string, string>> Improvement
        {
            get
            {
                if (_improvement == null)
                {
                    _improvement = new ObservableCollection<KeyValuePair<string, string>>();
                }
                return _improvement;
            }
            set => SetProperty(ref _improvement, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }

    public class LiquidProgress : ProgressBase
    {
        private List<bool> _goalCupCount;

        public List<bool> GoalCupCount
        {
            get => _goalCupCount;
            set => SetProperty(ref _goalCupCount, value);
        }

        private List<bool> _actualCupCount;

        public List<bool> ActualCupCount
        {
            get => _actualCupCount;
            set => SetProperty(ref _actualCupCount, value);
        }

        private double _goalCount;

        public double GoalCount
        {
            get => _goalCount;
            set => SetProperty(ref _goalCount, value);
        }

        private double _actualCount;

        public double ActualCount
        {
            get => _actualCount;
            set => SetProperty(ref _actualCount, value);
        }

        private string _titleIcon;

        public string TitleIcon
        {
            get => _titleIcon;
            set => SetProperty(ref _titleIcon, value);
        }

        private string _titleLabel;

        public string TitleLabel
        {
            get => _titleLabel;
            set => SetProperty(ref _titleLabel, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public LiquidProgress(string titleIcon, string titleLabel, double actualgoal, double targetgoal)
        {
            TitleIcon = titleIcon;
            TitleLabel = titleLabel;
            GoalCupCount = Enumerable.Range(1, 8).Select(X => false).ToList();
            ActualCupCount = Enumerable.Range(1, 8).Select(X => false).ToList();

            actualgoal = actualgoal / 0.25;
            targetgoal = targetgoal / 0.25;

            GoalCount = Math.Round(targetgoal, 2, MidpointRounding.AwayFromZero);
            ActualCount = Math.Round(actualgoal, 2, MidpointRounding.AwayFromZero);

            if (targetgoal > 0)
            {
                if (actualgoal > targetgoal)
                {
                    Score = Convert.ToInt32(Math.Round((100 * targetgoal) / actualgoal));
                }
                else
                {
                    Score = Convert.ToInt32(Math.Round((100 * actualgoal) / targetgoal));
                }
            }
            else
            {
                Score = 0;
            }

            if (Score >= 75)
            {
                Message = "Well done.";
            }
            else
            {
                Message = "You need to work on this.";
            }
            actualgoal = actualgoal > 8 ? 8 : actualgoal;
            targetgoal = targetgoal > 8 ? 8 : targetgoal;

            for (int i = 0; i < (int)actualgoal; i++)
            {
                ActualCupCount[i] = true;
            }
            for (int i = 0; i < (int)targetgoal; i++)
            {
                GoalCupCount[i] = true;
            }
        }
    }

    public class FoodProgress : ProgressBase
    {
        private string _titleIcon;

        public string TitleIcon
        {
            get => _titleIcon;
            set => SetProperty(ref _titleIcon, value);
        }

        private string _titleLabel;

        public string TitleLabel
        {
            get => _titleLabel;
            set => SetProperty(ref _titleLabel, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _actualFood;

        public string ActualFood
        {
            get => _actualFood;
            set => SetProperty(ref _actualFood, value);
        }

        private string _targetFood;

        public string TargetFood
        {
            get => _targetFood;
            set => SetProperty(ref _targetFood, value);
        }

        private bool _isVeryHeavyFood;

        public bool IsVeryHeavyFood
        {
            get => _isVeryHeavyFood;
            set => SetProperty(ref _isVeryHeavyFood, value);
        }

        private bool _isHeavyFood;

        public bool IsHeavyFood
        {
            get => _isHeavyFood;
            set => SetProperty(ref _isHeavyFood, value);
        }

        private bool _isMediumFood;

        public bool IsMediumFood
        {
            get => _isMediumFood;
            set => SetProperty(ref _isMediumFood, value);
        }

        private bool _isLightFood;

        public bool IsLightFood
        {
            get => _isLightFood;
            set => SetProperty(ref _isLightFood, value);
        }

        private bool _isVeryLightFood;

        public bool IsVeryLightFood
        {
            get => _isVeryLightFood;
            set => SetProperty(ref _isVeryLightFood, value);
        }

        private Color _goalColorType;

        public Color GoalColorType
        {
            get => _goalColorType;
            set => SetProperty(ref _goalColorType, value);
        }

        private Color _actualColorType;

        public Color ActualColorType
        {
            get => _actualColorType;
            set => SetProperty(ref _actualColorType, value);
        }

        public FoodProgress(string titleIcon, string titleLabel, FoodAmountType actualFood, FoodAmountType targetFood, double goal = 0, double actual = 0, bool isActivityType = false)
        {
            TitleIcon = titleIcon;
            TitleLabel = titleLabel;
            ActualFood = actualFood.ToString();
            TargetFood = targetFood.ToString();

            IsVeryLightFood = false;
            IsLightFood = false;
            IsMediumFood = false;
            IsHeavyFood = false;
            IsVeryHeavyFood = false;
            GoalColorType = Color.Transparent;
            int targetRating = 0;
            int actualRating = 0;
            switch (targetFood)
            {
                case FoodAmountType.Undefined:
                case FoodAmountType.None:
                    ActualColorType = CustomColors.None;
                    targetRating = 0;
                    break;

                case FoodAmountType.VeryLight:
                    IsVeryLightFood = true;
                    if (isActivityType)
                        GoalColorType = CustomColors.VeryHeavy;
                    else
                        GoalColorType = CustomColors.VeryLight;
                    targetRating = 20;

                    break;

                case FoodAmountType.Light:
                    IsLightFood = true;
                    if (isActivityType)
                        GoalColorType = CustomColors.Heavy;
                    else
                        GoalColorType = CustomColors.Light;
                    targetRating = 40;
                    break;

                case FoodAmountType.Medium:
                    IsMediumFood = true;
                    if (isActivityType)
                        GoalColorType = CustomColors.Medium;
                    else
                        GoalColorType = CustomColors.Medium;
                    targetRating = 60;
                    break;

                case FoodAmountType.Heavy:
                    IsHeavyFood = true;
                    if (isActivityType)
                        GoalColorType = CustomColors.VeryLight;
                    else
                        GoalColorType = CustomColors.VeryHeavy;
                    targetRating = 80;

                    break;

                case FoodAmountType.VeryHeavy:
                    IsVeryHeavyFood = true;
                    if (isActivityType)
                        GoalColorType = CustomColors.Light;
                    else
                        GoalColorType = CustomColors.VeryHeavy;
                    targetRating = 100;
                    break;

                default:
                    break;
            }

            IsActualVeryLightFood = false;
            IsActualLightFood = false;
            IsActualMediumFood = false;
            IsActualHeavyFood = false;
            IsActualVeryHeavyFood = false;
            ActualColorType = Color.Transparent;
            switch (actualFood)
            {
                case FoodAmountType.Undefined:
                case FoodAmountType.None:
                    ActualColorType = CustomColors.None;
                    actualRating = 0;
                    break;

                case FoodAmountType.VeryLight:
                    IsActualVeryLightFood = true;
                    if (isActivityType)
                        ActualColorType = CustomColors.VeryHeavy;
                    else
                        ActualColorType = CustomColors.VeryLight;
                    actualRating = 20;
                    break;

                case FoodAmountType.Light:
                    IsActualLightFood = true;
                    if (isActivityType)
                        ActualColorType = CustomColors.Heavy;
                    else
                        ActualColorType = CustomColors.Light;
                    actualRating = 40;
                    break;

                case FoodAmountType.Medium:
                    IsActualMediumFood = true;
                    if (isActivityType)
                        ActualColorType = CustomColors.Medium;
                    else
                        ActualColorType = CustomColors.Medium;
                    actualRating = 60;
                    break;

                case FoodAmountType.Heavy:
                    IsActualHeavyFood = true;
                    if (isActivityType)
                        ActualColorType = CustomColors.VeryLight;
                    else
                        ActualColorType = CustomColors.Heavy;
                    actualRating = 80;
                    break;

                case FoodAmountType.VeryHeavy:
                    IsActualVeryHeavyFood = true;
                    if (isActivityType)
                        ActualColorType = CustomColors.Light;
                    else
                        ActualColorType = CustomColors.VeryHeavy;
                    actualRating = 100;
                    break;

                default:
                    break;
            }

            if (targetRating > 0)
            {
                if (actualRating > targetRating)
                {
                    Score = Convert.ToInt32(Math.Round(Convert.ToDecimal((100 * targetRating) / actualRating)));
                }
                else
                {
                    Score = Convert.ToInt32(Math.Round(Convert.ToDecimal((100 * actualRating) / targetRating)));
                }
            }
            else
            {
                Score = 0;
            }

            if (goal > 0)
            {
                Score = 0;
                if (actual > goal && actual != 0)
                {
                    Score = Convert.ToInt32(Math.Round(Convert.ToDecimal((100 * goal) / actual)));
                }
                else
                {
                    Score = Convert.ToInt32(Math.Round(Convert.ToDecimal((100 * actual) / goal)));
                }
            }

            if (Score >= 75)
            {
                Message = "Well done.";
            }
            else
            {
                Message = "You need to work on this.";
            }
        }

        private bool _isActualVeryHeavyFood;

        public bool IsActualVeryHeavyFood
        {
            get => _isActualVeryHeavyFood;
            set => SetProperty(ref _isActualVeryHeavyFood, value);
        }

        private bool _isActualHeavyFood;

        public bool IsActualHeavyFood
        {
            get => _isActualHeavyFood;
            set => SetProperty(ref _isActualHeavyFood, value);
        }

        private bool _isActualMediumFood;

        public bool IsActualMediumFood
        {
            get => _isActualMediumFood;
            set => SetProperty(ref _isActualMediumFood, value);
        }

        private bool _isActualLightFood;

        public bool IsActualLightFood
        {
            get => _isActualLightFood;
            set => SetProperty(ref _isActualLightFood, value);
        }

        private bool _isActualVeryLightFood;

        public bool IsActualVeryLightFood
        {
            get => _isActualVeryLightFood;
            set => SetProperty(ref _isActualVeryLightFood, value);
        }


        private bool _isFastingProgressVisible;

        public bool IsFastingProgressVisible
        {
            get => _isFastingProgressVisible;
            set => SetProperty(ref _isFastingProgressVisible, value);
        }

    }

    public class MetabolicProgress : ProgressBase
    {
        public MetabolicProgress(int metabolicWeight)
        {
            Score = metabolicWeight;
            if (Score < 0)
            {
                IsMetabolicDeveiceActive = false;
            }
            else
            {
                IsMetabolicDeveiceActive = true;
            }
        }


        private bool _isMetabolicDeveiceActive;

        public bool IsMetabolicDeveiceActive
        {
            get => _isMetabolicDeveiceActive;
            set => SetProperty(ref _isMetabolicDeveiceActive, value);
        }

    }

    public abstract class ProgressBase : BindableBase
    {
        private int _score;

        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }
    }
}
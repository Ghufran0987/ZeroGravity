using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using ZeroGravity.Mobile.Proxies.MealIngredientsProxy;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto.MealIngredientsDto;

namespace ZeroGravity.Mobile.Proxies
{
    public class MealDataProxy : BindableBase
    {
        private DateTime _createDateTime;

        private int _foodAmount;

        private string _name;

        private TimeSpan _time;

        public MealDataProxy(MealSlotType type, FoodAmountType minAmount = FoodAmountType.None) : this(type,
            new List<MealIngredientsBaseDto>(), minAmount)
        {
        }

        public MealDataProxy(MealSlotType type, List<MealIngredientsBaseDto> ingredients,
            FoodAmountType minAmount = FoodAmountType.None)
        {
            MealSlotType = type;
            IsActive = true;
            MinAmount = (int)minAmount;
            MaxAmount = (int)FoodAmountType.VeryHeavy;

            Ingredients = new List<MealIngredientsBaseProxy>();

            foreach (var ingredient in ingredients)
            {
                switch (ingredient)
                {
                    case ProteinDto protein:
                        Ingredients.Add(new ProteinProxy { Amount = protein.Amount });
                        break;
                    case DairyDto dairy:
                        Ingredients.Add(new DairyProxy { Amount = dairy.Amount });
                        break;
                    case FruitsDto fruits:
                        Ingredients.Add(new FruitsProxy { Amount = fruits.Amount });
                        break;
                    case GrainsDto grains:
                        Ingredients.Add(new GrainsProxy { Amount = grains.Amount });
                        break;
                    case VegetablesDto vegetables:
                        Ingredients.Add(new VegetablesProxy { Amount = vegetables.Amount });
                        break;
                }
            }
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public MealSlotType MealSlotType { get; }

        public int MinAmount { get; set; }

        public int MaxAmount { get; set; }

        public List<MealIngredientsBaseProxy> Ingredients { get; set; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public DateTime CreateDateTime
        {
            get => _createDateTime;
            set => SetProperty(ref _createDateTime, value);
        }

        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public int FoodAmount
        {
            get => _foodAmount;
            set => SetProperty(ref _foodAmount, value);
        }



        public bool IsFoodGroupActive
        {
            get { return MealSlotType == MealSlotType.Breakfast || MealSlotType == MealSlotType.Lunch || MealSlotType == MealSlotType.Dinner ? true : false; }

        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }


        private bool _hasIngredientDairy;
        private bool _hasIngredientFruits;
        private bool _hasIngredientGrains;
        private bool _hasIngredientProtein;
        private bool _hasIngredientVegetables;

        public bool UpdateNeeded;

        public bool HasIngredientGrains
        {
            get => _hasIngredientGrains;
            set => SetProperty(ref _hasIngredientGrains, value);
        }

        public bool HasIngredientVegetables
        {
            get => _hasIngredientVegetables;
            set => SetProperty(ref _hasIngredientVegetables, value);
        }

        public bool HasIngredientFruits
        {
            get => _hasIngredientFruits;
            set => SetProperty(ref _hasIngredientFruits, value);
        }

        public bool HasIngredientDairy
        {
            get => _hasIngredientDairy;
            set => SetProperty(ref _hasIngredientDairy, value);
        }

        public bool HasIngredientProtein
        {
            get => _hasIngredientProtein;
            set => SetProperty(ref _hasIngredientProtein, value);
        }

        public void UpdateMealData(MealDataProxy meal)
        {
            Id = meal.Id;
            Ingredients = meal.Ingredients;
            AccountId = meal.AccountId;
            MinAmount = meal.MinAmount;
            MaxAmount = meal.MaxAmount;
            Name = meal.Name;
            CreateDateTime = meal.CreateDateTime;
            UpdateNeeded = true;
            FoodAmount = meal.FoodAmount;
            foreach (var item in meal.Ingredients)
            {
                switch (item)
                {
                    case ProteinProxy protein:
                        HasIngredientProtein = true;
                        break;
                    case DairyProxy dairy:
                        HasIngredientDairy = true;
                        break;
                    case FruitsProxy fruits:
                        HasIngredientFruits = true;
                        break;
                    case GrainsProxy grains:
                        HasIngredientGrains = true;
                        break;
                    case VegetablesProxy vegetables:
                        HasIngredientVegetables = true;
                        break;
                }
            }
        }

        public void UpdateIngredients()
        {

            foreach (var item in Ingredients)
            {
                switch (item)
                {
                    case ProteinProxy protein:
                        HasIngredientProtein = true;
                        break;
                    case DairyProxy dairy:
                        HasIngredientDairy = true;
                        break;
                    case FruitsProxy fruits:
                        HasIngredientFruits = true;
                        break;
                    case GrainsProxy grains:
                        HasIngredientGrains = true;
                        break;
                    case VegetablesProxy vegetables:
                        HasIngredientVegetables = true;
                        break;
                }
            }
        }

        public void UpdateIngredientStatus(bool status)
        {
            HasIngredientProtein = status;
            HasIngredientDairy = status;
            HasIngredientFruits = status;
            HasIngredientGrains = status;
            HasIngredientVegetables = status;
            IsActive = status;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Models.MealIngredients;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId), nameof(Created))]
    public class MealData : ModelBase
    {
        [Required] 
        public int AccountId { get; set; }

        //Todo klären wie der Name der Mahlzeit hinterlegt wird
        public string Name { get; set; }

        public MealSlotType MealSlotType { get; set; }

        public DateTime Created { get; set; }

        public FoodAmountType Amount { get; set; }

        public int Quantity { get; set; }

        public List<MealIngredientsBase> Ingredients { get; set; }

        public MealData()
        {
            Ingredients = new List<MealIngredientsBase>();
        }
    }
}
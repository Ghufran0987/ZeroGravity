using System;
using System.Collections.Generic;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto.MealIngredientsDto;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MealDataDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public MealSlotType MealSlotType { get; set; }

        public DateTime Created { get; set; }

        public FoodAmountType Amount { get; set; }

        public int Quantity { get; set; }

        public List<MealIngredientsBaseDto> Ingredients { get; set; }
    }
}
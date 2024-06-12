using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class TrackedHistoryDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public HistoryItemType HistoryItemType { get; set; }

        public string Duration { get; set; }

        public double Amount { get; set; }

        public DateTime Created { get; set; }

        public FoodAmountType FoodAmountType { get; set; }

        public WellbeingType WellbeingType { get; set; }
    }
}
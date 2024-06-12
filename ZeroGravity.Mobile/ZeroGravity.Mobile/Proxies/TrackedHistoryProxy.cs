using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class TrackedHistoryProxy : ProxyBase
    {
        public int AccountId { get; set; }

        public HistoryItemType HistoryItemType { get; set; }

        public string Description { get; set; }

        public FoodAmountType FoodAmountType { get; set; }

        public WellbeingType WellbeingType { get; set; }
    }
}
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IHoldingPagesSettingsService
    {
        bool ShouldShow(HoldingPageType holdingPageType);
        void DoNotShowAgain(HoldingPageType holdingPageType);
        bool ShouldDailyShow(HoldingPageType holdingPageType);
        void DoNotShowToday(HoldingPageType holdingPageType);

    }
}

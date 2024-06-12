using System;
using System.Collections.Generic;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Services
{
    public class HoldingPagesSettingsService : IHoldingPagesSettingsService
    {
        private readonly ISecureStorageService _secureStorage;

        public HoldingPagesSettingsService(ISecureStorageService secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public bool ShouldShow(HoldingPageType holdingPageType)
        {
            var savedSettings = _secureStorage.LoadObject<List<HoldingPageType>>(SecureStorageKey.HoldingPagesSettings).Result;

            if (savedSettings == null)
            {
                return true;
            }

            var contains = savedSettings.Contains(holdingPageType);

            return !contains;
        }

        public void DoNotShowAgain(HoldingPageType holdingPageType)
        {
            var savedSettings = _secureStorage.LoadObject<List<HoldingPageType>>(SecureStorageKey.HoldingPagesSettings)
                .Result;

            if (savedSettings == null || savedSettings.Contains(holdingPageType))
            {
                savedSettings = new List<HoldingPageType>();
            }

            savedSettings.Add(holdingPageType);

            _secureStorage.SaveObject(SecureStorageKey.HoldingPagesSettings, savedSettings);
        }


        public bool ShouldDailyShow(HoldingPageType holdingPageType)
        {
            var savedSettings = _secureStorage.LoadObject<Dictionary<HoldingPageType, DateTime>>(SecureStorageKey.DailyHoldingPagesSettings).Result;

            if (savedSettings == null)
            {
                return true;
            }

            if (savedSettings.ContainsKey(holdingPageType))
            {
                var resultdate = savedSettings[holdingPageType];
                if (resultdate.Date >= DateTime.Today) // .AddDays(1) For Testing
                {
                    return false;
                }
                else
                    return true;
            }
            return true;
        }

        public void DoNotShowToday(HoldingPageType holdingPageType)
        {
            var savedSettings = _secureStorage.LoadObject<Dictionary<HoldingPageType, DateTime>>(SecureStorageKey.DailyHoldingPagesSettings)
                .Result;

            if (savedSettings == null)
            {
                savedSettings = new Dictionary<HoldingPageType, DateTime>();
                savedSettings.Add(holdingPageType, DateTime.Today);
            }
            else if (savedSettings.ContainsKey(holdingPageType))
            {
                savedSettings[holdingPageType] = DateTime.Today;
            }
            else
            {
                savedSettings.Add(holdingPageType, DateTime.Today);
            }

            _secureStorage.SaveObject(SecureStorageKey.DailyHoldingPagesSettings, savedSettings);
        }
    }
}

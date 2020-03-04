using System;
using System.Collections.Generic;
using System.Linq;

namespace YouScan.Sale
{
    public class DiscountService: IDiscountService
    {
        private IReadOnlyCollection<DiscountSettings> _discountSettings;

        public void SetDiscountSettings(IReadOnlyCollection<DiscountSettings> discountSettings)
        {
            if (discountSettings == null || !discountSettings.Any())
            {
                throw new ArgumentNullException(nameof(discountSettings));
            }

            _discountSettings = discountSettings;
        }

        public double CalculatePurchaseAmountWithDiscount(DiscountCard card, double purchaseAmount)
        {
            var settings = GetSettings(card);
            if (settings == null)
            {
                return purchaseAmount;
            }
            else
            {
                return purchaseAmount * (100.00 - settings.Percentage) / 100.00;
            }
        }

        public double CalculateDiscountAmount(DiscountCard card, double purchaseAmount)
        {
            var settings = GetSettings(card);
            if (settings == null)
            {
                return purchaseAmount;
            }
            else
            {
                return purchaseAmount * settings.Percentage / 100.00;
            }
        }

        public double GetDiscountPercentage(DiscountCard card)
        {
            var settings = GetSettings(card);
            return settings?.Percentage ?? 0;
        }

        private DiscountSettings GetSettings(DiscountCard card)
        {
            var fullAmount = card.GetFullAmount();
            var settings = _discountSettings.FirstOrDefault(x => fullAmount >= x.MinAmount && (fullAmount <= x.MaxAmount || x.MaxAmount == null));
            return settings;
        }
    }
}
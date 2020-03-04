using System.Collections.Generic;

namespace YouScan.Sale
{
    public interface IDiscountService
    {
        void SetDiscountSettings(IReadOnlyCollection<DiscountSettings> discountSettings);
        double CalculatePurchaseAmountWithDiscount(DiscountCard card, double purchaseAmount);
        double GetDiscountPercentage(DiscountCard card);
        double CalculateDiscountAmount(DiscountCard card, double purchaseAmount);
    }
}
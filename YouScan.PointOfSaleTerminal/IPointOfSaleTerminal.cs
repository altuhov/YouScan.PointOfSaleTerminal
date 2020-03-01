using System.Collections.Generic;

namespace YouScan.Sale
{
    public interface IPointOfSaleTerminal
    {
        void SetPricing(IReadOnlyDictionary<string, ProductPricingSettings> pricingSettings);
        void Scan(string productCode);
        double CalculateTotal();
    }
}
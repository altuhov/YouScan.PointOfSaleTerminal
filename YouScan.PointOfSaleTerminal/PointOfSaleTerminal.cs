using System;
using System.Collections.Generic;
using System.Linq;

namespace YouScan.Sale
{
    public class PointOfSaleTerminal : IPointOfSaleTerminal
    {
        private readonly IList<string> _productCodes = new List<string>();
        private IReadOnlyDictionary<string, ProductPricingSettings> _pricingSettings;
        private IReadOnlyDictionary<string, ProductPricingSettings> _pricingSettingsForDiscountProducts => GetPricingSettingsForDiscountProducts();

        public double CalculateTotal()
        {
            if (_pricingSettings == null || !_pricingSettings.Any())
            {
                throw new ArgumentException("PricingSettings can not be null or empty");
            }

            if (!_productCodes.Any())
            {
                return 0;
            }

            var countProductsByProductCode = _productCodes.GroupBy(productCode => productCode, (productCode, productCodes) => new { ProductCode = productCode, Count = productCodes.Count() });

            double result = countProductsByProductCode.Sum(arg => CalculateByProductCode(arg.ProductCode, arg.Count));

            return result;
        }

        public double CalculateForDiscount()
        {
            if (!_pricingSettingsForDiscountProducts.Any() || !_productCodes.Any())
            {
                return 0;
            }

            List<string> productsForDiscount = _productCodes.Where(x => _pricingSettingsForDiscountProducts.ContainsKey(x))
                                                  .ToList();

            if (!productsForDiscount.Any())
            {
                return 0;
            }

            var countProductsByProductCode = productsForDiscount.GroupBy(productCode => productCode, (productCode, productCodes) => new { ProductCode = productCode, Count = productCodes.Count() });

            double result = countProductsByProductCode.Sum(arg => CalculateByProductCode(arg.ProductCode, arg.Count));
            return result;
        }

        public void Scan(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                throw new ArgumentNullException(nameof(productCode));
            }

            _productCodes.Add(productCode);
        }

        public void SetPricing(IReadOnlyDictionary<string, ProductPricingSettings> pricingSettings)
        {
            if (pricingSettings == null || !pricingSettings.Any())
            {
                throw new ArgumentException("PricingSettings can not be null or empty");
            }

            _pricingSettings = pricingSettings;
        }

        private double CalculateByProductCode(string productCode, int productsCount)
        {
            if (!_pricingSettings.TryGetValue(productCode, out ProductPricingSettings pricingSettings))
            {
                throw new Exception($"PricingSettings were not found for {productCode}");
            }

            int perUnitCount = pricingSettings.VolumeItems == 0 ? productsCount : productsCount % pricingSettings.VolumeItems;
            int packCount = pricingSettings.VolumeItems == 0 ? 0 : productsCount / pricingSettings.VolumeItems;

            double result = pricingSettings.VolumePrice * packCount + pricingSettings.PerUnitPrice * perUnitCount;

            return result;
        }


        private IReadOnlyDictionary<string, ProductPricingSettings> GetPricingSettingsForDiscountProducts()
        {
            if (_pricingSettings == null || !_pricingSettings.Any())
            {
                return new Dictionary<string, ProductPricingSettings>();
            }

            var res = _pricingSettings.Where(x => x.Value.VolumeItems == 0)
                                      .ToDictionary(pair => pair.Key, pair => pair.Value);

            return res;
        }
    }
}
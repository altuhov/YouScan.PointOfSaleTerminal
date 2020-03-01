namespace YouScan.Sale
{
    public class ProductPricingSettings
    {
        public ProductPricingSettings(double perUnitPrice): this(perUnitPrice, 0, 0)
        {
            
        }

        public ProductPricingSettings(double perUnitPrice, double volumePrice, int volumeItems)
        {
            PerUnitPrice = perUnitPrice;
            VolumePrice = volumePrice;
            VolumeItems = volumeItems;
        }

        public double PerUnitPrice { get; }
        public double VolumePrice { get; }
        public int VolumeItems { get; }
    }
}
namespace YouScan.Sale
{
    public class DiscountCard
    {
        private double _amount = 0;

        public void AddAmountForDiscount(double amount)
        {
            _amount += amount;
        }

        public double GetFullAmount()
        {
            return _amount;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace YouScan.Sale.UnitTests
{
    public class DiscountServiceTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 1)]
        [InlineData(1999, 1)]
        [InlineData(2000, 3)]
        [InlineData(4999, 3)]
        [InlineData(5000, 4)]
        [InlineData(9999, 4)]
        [InlineData(100000, 5)]
        [InlineData(10000, 5)]
        public void CalculateDiscount_Should_Return_Correct_Sum(double amount, double percentage)
        {
            // arrange
            IDiscountService discountService = new DiscountService();

            IReadOnlyCollection<DiscountSettings> settings = new List<DiscountSettings>()
            {
                new DiscountSettings(){Percentage = 1, MinAmount = 1000, MaxAmount = 1999},
                new DiscountSettings(){Percentage = 3, MinAmount = 2000, MaxAmount = 4999},
                new DiscountSettings(){Percentage = 4, MinAmount = 5000, MaxAmount = 9999},
                new DiscountSettings(){Percentage = 5, MinAmount = 10000, MaxAmount = null}
            };

            discountService.SetDiscountSettings(settings);
            var card = new DiscountCard();
            card.AddAmountForDiscount(amount);

            // act
            double result = discountService.GetDiscountPercentage(card);

            // assert
            Assert.Equal(percentage, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 990)]
        [InlineData(1999, 1979.01)]
        [InlineData(2000, 1940)]
        [InlineData(4999, 4849.03)]
        [InlineData(5000, 4800)]
        [InlineData(9999, 9599.04)]
        [InlineData(100000, 95000)]
        [InlineData(10000, 9500)]
        public void CalculatePurchaseAmountWithDiscount_Should_Return_Correct_Sum(double purchaseAmount, double purchaseAmountWithDiscount)
        {
            // arrange
            IDiscountService discountService = new DiscountService();

            IReadOnlyCollection<DiscountSettings> settings = new List<DiscountSettings>()
            {
                new DiscountSettings(){Percentage = 1, MinAmount = 1000, MaxAmount = 1999},
                new DiscountSettings(){Percentage = 3, MinAmount = 2000, MaxAmount = 4999},
                new DiscountSettings(){Percentage = 4, MinAmount = 5000, MaxAmount = 9999},
                new DiscountSettings(){Percentage = 5, MinAmount = 10000, MaxAmount = null}
            };

            discountService.SetDiscountSettings(settings);
            var card = new DiscountCard();
            card.AddAmountForDiscount(purchaseAmount);

            // act
            double result = discountService.CalculatePurchaseAmountWithDiscount(card, purchaseAmount);

            // assert
            Assert.Equal(purchaseAmountWithDiscount, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 10)]
        [InlineData(1999, 19.99)]
        [InlineData(2000, 60)]
        [InlineData(4999, 149.97)]
        [InlineData(5000, 200)]
        [InlineData(9999, 399.96)]
        [InlineData(100000, 5000)]
        [InlineData(10000, 500)]
        public void CalculateDiscountAmount_Should_Return_Correct_Sum(double purchaseAmount, double discountAmount)
        {
            // arrange
            IDiscountService discountService = new DiscountService();

            IReadOnlyCollection<DiscountSettings> settings = new List<DiscountSettings>()
            {
                new DiscountSettings(){Percentage = 1, MinAmount = 1000, MaxAmount = 1999},
                new DiscountSettings(){Percentage = 3, MinAmount = 2000, MaxAmount = 4999},
                new DiscountSettings(){Percentage = 4, MinAmount = 5000, MaxAmount = 9999},
                new DiscountSettings(){Percentage = 5, MinAmount = 10000, MaxAmount = null}
            };

            discountService.SetDiscountSettings(settings);
            var card = new DiscountCard();
            card.AddAmountForDiscount(purchaseAmount);

            // act
            double result = discountService.CalculateDiscountAmount(card, purchaseAmount);

            // assert
            Assert.Equal(discountAmount, result);
        }

    }
}

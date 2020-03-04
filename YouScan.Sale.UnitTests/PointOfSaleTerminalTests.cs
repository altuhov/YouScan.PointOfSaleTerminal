using System;
using System.Collections.Generic;
using Xunit;

namespace YouScan.Sale.UnitTests
{
    public class PointOfSaleTerminalTests
    {
        [Fact]
        public void CalculateTotal_Should_Return_0_If_No_Scan()
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();
            IReadOnlyDictionary<string, ProductPricingSettings> settings = new Dictionary<string, ProductPricingSettings>
            {
                { "A", new ProductPricingSettings(1.25, 3, 3) },
                { "B", new ProductPricingSettings(4.25) },
                { "C", new ProductPricingSettings(1, 5, 6) },
                { "D", new ProductPricingSettings(0.75) }
            };
            terminal.SetPricing(settings);

            // act
            double result = terminal.CalculateTotal();

            // assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("ABCDABA", 13.25)]
        [InlineData("CCCCCCC", 6.00)]
        [InlineData("ABCD", 7.25)]
        [InlineData("ABCDABCD", 14.5)]
        [InlineData("E", 0.75)]
        [InlineData("EE", 1.5)]
        public void CalculateTotal_Should_Return_Correct_Sum(string productCodes, double totalPrice)
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();
            IReadOnlyDictionary<string, ProductPricingSettings> settings = new Dictionary<string, ProductPricingSettings>
            {
                { "A", new ProductPricingSettings(1.25, 3, 3) },
                { "B", new ProductPricingSettings(4.25) },
                { "C", new ProductPricingSettings(1, 5, 6) },
                { "D", new ProductPricingSettings(0.75) },
                { "E", new ProductPricingSettings(1, 0.75, 1) }
            };
            terminal.SetPricing(settings);

            foreach (char productCode in productCodes)
            {
                terminal.Scan(productCode.ToString());
            }

            // act
            double result = terminal.CalculateTotal();

            // assert
            Assert.Equal(totalPrice, result);
        }

        [Fact]
        public void CalculateTotal_Should_Throw_ArgumentException_If_ProductPricingSettings_Is_Null_or_Empty()
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();

            // assert
            Assert.Throws<ArgumentException>(() => terminal.CalculateTotal());
        }

        [Theory]
        [InlineData("F")]
        [InlineData("ABCDE")]
        public void CalculateTotal_Should_Throw_Exception_If_ProductPricingSettings_Were_Not_Found_For_Product(string productCodes)
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();
            IReadOnlyDictionary<string, ProductPricingSettings> settings = new Dictionary<string, ProductPricingSettings>
            {
                { "A", new ProductPricingSettings(1.25, 3, 3) },
                { "B", new ProductPricingSettings(4.25) },
                { "C", new ProductPricingSettings(1, 5, 6) },
                { "D", new ProductPricingSettings(0.75) }
            };
            terminal.SetPricing(settings);

            foreach (char productCode in productCodes)
            {
                terminal.Scan(productCode.ToString());
            }


            // assert
            Assert.Throws<Exception>(() => terminal.CalculateTotal());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Scan_Should_Throw_ArgumentNullException_If_ProductCode_Is_Null_or_Empty(string productCode)
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();

            // assert
            Assert.Throws<ArgumentNullException>(() => terminal.Scan(productCode));
        }

        [Fact]
        public void SetPricing_Should_Throw_ArgumentException_If_ProductPricingSettings_Is_Null_or_Empty()
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();

            // assert
            Assert.Throws<ArgumentException>(() => terminal.SetPricing(null));
            Assert.Throws<ArgumentException>(() => terminal.SetPricing(new Dictionary<string, ProductPricingSettings>()));
        }

        [Theory]
        [InlineData("ABCDABA", 9.25)]
        [InlineData("CCCCCCC", 0)]
        [InlineData("ABCD", 5)]
        [InlineData("ABCDABCD", 10)]
        [InlineData("E", 0)]
        [InlineData("EE", 0)]
        [InlineData("ABCDABCDFFF", 10)]
        public void CalculateForDiscount_Should_Return_Correct_Sum(string productCodes, double totalPrice)
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();
            IReadOnlyDictionary<string, ProductPricingSettings> settings = new Dictionary<string, ProductPricingSettings>
            {
                { "A", new ProductPricingSettings(1.25, 3, 3) },
                { "B", new ProductPricingSettings(4.25) },
                { "C", new ProductPricingSettings(1, 5, 6) },
                { "D", new ProductPricingSettings(0.75) },
                { "E", new ProductPricingSettings(1, 0.75, 1) }
            };
            terminal.SetPricing(settings);

            foreach (char productCode in productCodes)
            {
                terminal.Scan(productCode.ToString());
            }

            // act
            double result = terminal.CalculateForDiscount();

            // assert
            Assert.Equal(totalPrice, result);
        }

        [Fact]
        public void CalculateForDiscount_Should_Return_0_If_No_Scan()
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();
            IReadOnlyDictionary<string, ProductPricingSettings> settings = new Dictionary<string, ProductPricingSettings>
            {
                { "A", new ProductPricingSettings(1.25, 3, 3) },
                { "B", new ProductPricingSettings(4.25) },
                { "C", new ProductPricingSettings(1, 5, 6) },
                { "D", new ProductPricingSettings(0.75) }
            };
            terminal.SetPricing(settings);

            // act
            double result = terminal.CalculateForDiscount();

            // assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateForDiscount_Should_Return_0_If_ProductPricingSettings_Is_Null_or_Empty()
        {
            // arrange
            IPointOfSaleTerminal terminal = new PointOfSaleTerminal();

            // act
            double result = terminal.CalculateForDiscount();

            // assert
            Assert.Equal(0, result);
        }
    }
}
using Xunit;

namespace PROG7311.Tests
{
    public class CurrencyCalculationTests
    {
        [Fact]
        public void CurrencyConversion_CorrectAmount()
        {
            decimal usdAmount = 100;
            decimal exchangeRate = 18.50m;
            decimal expected = 1850.00m;

            decimal result = usdAmount * exchangeRate;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CurrencyConversion_ZeroAmount_ReturnsZero()
        {
            decimal usdAmount = 0;
            decimal exchangeRate = 18.50m;

            decimal result = usdAmount * exchangeRate;

            Assert.Equal(0, result);
        }

        [Fact]
        public void CurrencyConversion_NegativeAmount_ReturnsNegative()
        {
            decimal usdAmount = -50;
            decimal exchangeRate = 18.50m;

            decimal result = usdAmount * exchangeRate;

            Assert.True(result < 0);
        }

        [Fact]
        public void CurrencyConversion_LargeAmount_CalculatesCorrectly()
        {
            decimal usdAmount = 10000;
            decimal exchangeRate = 18.50m;
            decimal expected = 185000.00m;

            decimal result = usdAmount * exchangeRate;

            Assert.Equal(expected, result);
        }
    }
}
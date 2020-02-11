using System;
using NUnit.Framework;

namespace Honey.Tests
{
    public class CurrencyTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Constructor_ThrowsArgumentException(string currencyCode)
        {
            Assert.Throws<ArgumentException>(() => new Currency(currencyCode));
        }

        [Test]
        public void Equals_ReturnsTrue_WhenCurrencyCodeIsTheSame()
        {
            var currency1 = new Currency("USD");
            var currency2 = new Currency("USD");
            
            Assert.IsTrue(currency1 == currency2);
            Assert.IsTrue(currency1.Equals(currency2));
            Assert.IsTrue(currency2.Equals(currency1));
            Assert.AreEqual(currency1, currency2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyCodeIsDifferent()
        {
            var currency1 = new Currency("USD");
            var currency2 = new Currency("EUR");
            
            Assert.IsTrue(currency1 != currency2);
            Assert.IsFalse(currency1.Equals(currency2));
            Assert.IsFalse(currency2.Equals(currency1));
            Assert.AreNotEqual(currency1, currency2);
        }

        [Test]
        public void ToString_ReturnsCurrencyCode()
        {
            const string currencyCode = "USD";
            var currency = new Currency(currencyCode);
            
            Assert.AreEqual(currencyCode, currency.ToString());
        }
    }
}
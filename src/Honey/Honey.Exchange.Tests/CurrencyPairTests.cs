using NUnit.Framework;

namespace Honey.Exchange.Tests
{
    public class CurrencyPairTests
    {
        readonly Currency EUR = new Currency("EUR");
        readonly Currency USD = new Currency("USD");
        
        [Test]
        public void Constructor_CreatesPairFromBaseAndQuoteCurrencies()
        {
            var pair = new CurrencyPair(EUR, USD);
            
            Assert.AreEqual(EUR, pair.BaseCurrency);
            Assert.AreEqual(USD, pair.QuoteCurrency);
        }
        
        [Test]
        public void Equals_ReturnsTrue_WhenCurrenciesAreTheSame()
        {
            var pair1 = new CurrencyPair(EUR, USD);
            var pair2 = new CurrencyPair(EUR, USD);
            
            Assert.IsTrue(pair1 == pair2);
            Assert.IsTrue(pair1.Equals(pair2));
            Assert.IsTrue(pair2.Equals(pair1));
            Assert.AreEqual(pair1, pair2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrenciesAreDifferent()
        {
            var pair1 = new CurrencyPair(EUR, USD);
            var pair2 = new CurrencyPair(USD, EUR);
            
            Assert.IsTrue(pair1 != pair2);
            Assert.IsFalse(pair1.Equals(pair2));
            Assert.IsFalse(pair2.Equals(pair1));
            Assert.AreNotEqual(pair1, pair2);
        }
        
        [Test]
        public void ToString_ReturnsCurrenciesSeparatedBySlash()
        {
            var pair = new CurrencyPair(EUR, USD);
            
            Assert.AreEqual("EUR/USD", pair.ToString());
        }
    }
}
using System;
using NUnit.Framework;

namespace Honey.Exchange.Tests
{
    public class CurrencyPairTests
    {
        static readonly Currency EUR = new Currency("EUR");
        static readonly Currency USD = new Currency("USD");
        
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

        [Test]
        public void Swap_ReturnsReverseCurrencyPair()
        {
            var pair = new CurrencyPair(EUR, USD);
            var expected = new CurrencyPair(USD, EUR);
            
            Assert.AreEqual(expected, pair.Swap());
        }

        private static TestCaseData[] _parseValid =
        {
            new TestCaseData(new CurrencyPair(EUR, USD), "EUR/USD"),
            new TestCaseData(new CurrencyPair(USD, EUR), "USD/EUR")
        };
        [TestCaseSource(nameof(_parseValid))]
        public void Parse_ReturnsCurrencyPairFromValidString(CurrencyPair pair, string s)
        {
            Assert.AreEqual(pair, CurrencyPair.Parse(s));
        }

        private static string[] _parseInvalid =
        {
            "USD/",
            "/USD",
            "USD",
            "USD ",
            "USD-EUR",
            "USD,EUR"
        };
        [TestCaseSource(nameof(_parseInvalid))]
        public void Parse_ThrowsFormatException_WhenStringIsNotValid(string s)
        {
            Assert.Throws<FormatException>(() => CurrencyPair.Parse(s));
        }

        [Test]
        public void Parse_ThrowsArgumentNullException_WhenStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => CurrencyPair.Parse(null));
        }
    }
}
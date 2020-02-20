using System;
using NUnit.Framework;

namespace Honey.Exchange.Tests
{
    public class ExchangeRateTests
    {
        readonly Currency EUR = new Currency("EUR");
        readonly Currency USD = new Currency("USD");
        readonly Currency GBP = new Currency("GBP");
        
        [Test]
        public void Constructor_CreatesExchangeRate()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(pair, 1.08m);
            
            Assert.AreEqual(pair, rate.Pair);
            Assert.AreEqual(1.08m, rate.Price);
        }

        [Test]
        public void Constructor_ThrowsInvalidPriceException_WhenPriceIsZero()
        {
            var pair = new CurrencyPair(EUR, USD);
            
            var ex = Assert.Throws<InvalidPriceException>(() => 
            {
                var rate = new ExchangeRate(pair, 0m);
            });
            Assert.AreEqual("Price cannot be zero", ex.Message);
        }

        [Test]
        public void Exchange_ReturnsMoneyInQuoteCurrencyWithMultipliedAmount()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(pair, 1.08m);
            var moneyToSell = new Money(10m, EUR);

            Assert.AreEqual(USD, rate.Exchange(moneyToSell).Currency);
            Assert.AreEqual(10.8m, rate.Exchange(moneyToSell).Amount);
        }

        [Test]
        public void Exchange_ThrowsInvalidCurrencyException_WhenPassedCurrencyIsNotBaseCurrency()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(pair, 1.08m);
            var moneyToSell = new Money(10m, USD);

            Assert.Throws<InvalidCurrencyException>(() => rate.Exchange(moneyToSell));
        }
        
        [Test]
        public void Equals_ReturnsTrue_WhenCurrencyPairAndPriceAreTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsTrue(rate1 == rate2);
            Assert.IsTrue(rate1.Equals(rate2));
            Assert.IsTrue(rate2.Equals(rate1));
            Assert.AreEqual(rate1, rate2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyPairIsTheSameButPriceIsDifferent()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            
            Assert.IsTrue(rate1 != rate2);
            Assert.IsFalse(rate1.Equals(rate2));
            Assert.IsFalse(rate2.Equals(rate1));
            Assert.AreNotEqual(rate1, rate2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyPairIsDifferentButPriceIsTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 10m);
            
            Assert.IsTrue(rate1 != rate2);
            Assert.IsFalse(rate1.Equals(rate2));
            Assert.IsFalse(rate2.Equals(rate1));
            Assert.AreNotEqual(rate1, rate2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyPairAndPriceAreDifferent()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 11m);
            
            Assert.IsTrue(rate1 != rate2);
            Assert.IsFalse(rate1.Equals(rate2));
            Assert.IsFalse(rate2.Equals(rate1));
            Assert.AreNotEqual(rate1, rate2);
        }

        [Test]
        public void Compare_ThrowsException_WhenCurrencyPairsMismatched()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 11m);
            
            Assert.Throws<InvalidCurrencyPairException>(() => rate1.CompareTo(rate2));
        }
        
        [Test]
        public void Compare_ReturnsMinusOne_WhenPriceIsLess()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            
            Assert.AreEqual(-1, rate1.CompareTo(rate2));
        }
        
        [Test]
        public void Compare_ReturnsZero_WhenPriceIsTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.AreEqual(0, rate1.CompareTo(rate2));
        }
        
        [Test]
        public void Compare_ReturnsPlusOne_WhenPriceIsGreater()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.AreEqual(1, rate1.CompareTo(rate2));
        }
        
        [Test]
        public void GreaterThan_ReturnsTrue_WhenPriceIsGreater()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsTrue(rate1 > rate2);
        }
        
        [Test]
        public void GreaterThan_ReturnsFalse_WhenPriceIsTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsFalse(rate1 > rate2);
        }
        
        [Test]
        public void GreaterThan_ReturnsFalse_WhenPriceIsLess()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            
            Assert.IsFalse(rate1 > rate2);
        }
        
        [Test]
        public void LessThan_ReturnsTrue_WhenPriceIsLess()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            
            Assert.IsTrue(rate1 < rate2);
        }
        
        [Test]
        public void LessThan_ReturnsFalse_WhenPriceIsTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsFalse(rate1 < rate2);
        }
        
        [Test]
        public void LessThan_ReturnsFalse_WhenPriceIsGreater()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsFalse(rate1 < rate2);
        }
        
        [Test]
        public void GreaterThan_ThrowsException_WhenCurrencyPairsMismatched()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 10m);

            Assert.Throws<InvalidCurrencyPairException>(() =>
            {
                var result = rate1 > rate2;
            });
        }
        
        [Test]
        public void LessThan_ThrowsException_WhenCurrencyPairsMismatched()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 11m);
            
            Assert.Throws<InvalidCurrencyPairException>(() =>
            {
                var result = rate1 < rate2;
            });
        }
        
        [Test]
        public void GreaterOrEqualThan_ReturnsTrue_WhenPriceIsGreater()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsTrue(rate1 >= rate2);
        }
        
        [Test]
        public void GreaterOrEqualThan_ReturnsTrue_WhenPriceIsTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsTrue(rate1 >= rate2);
        }
        
        [Test]
        public void GreaterOrEqualThan_ReturnsFalse_WhenPriceIsLess()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            
            Assert.IsFalse(rate1 >= rate2);
        }
        
        [Test]
        public void LessOrEqualThan_ReturnsTrue_WhenPriceIsLess()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            
            Assert.IsTrue(rate1 <= rate2);
        }
        
        [Test]
        public void LessOrEqualThan_ReturnsTrue_WhenPriceIsTheSame()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsTrue(rate1 <= rate2);
        }
        
        [Test]
        public void LessOrEqualThan_ReturnsFalse_WhenPriceIsGreater()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            
            Assert.IsFalse(rate1 <= rate2);
        }
        
        [Test]
        public void GreaterOrEqualThan_ThrowsException_WhenCurrencyPairsMismatched()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 11m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 10m);

            Assert.Throws<InvalidCurrencyPairException>(() =>
            {
                var result = rate1 >= rate2;
            });
        }
        
        [Test]
        public void LessOrEqualThan_ThrowsException_WhenCurrencyPairsMismatched()
        {
            var rate1 = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);
            var rate2 = new ExchangeRate(new CurrencyPair(USD, GBP), 11m);
            
            Assert.Throws<InvalidCurrencyPairException>(() =>
            {
                var result = rate1 <= rate2;
            });
        }
    }
}
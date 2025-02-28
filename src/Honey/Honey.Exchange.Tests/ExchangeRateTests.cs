using System;
using System.Linq;
using NUnit.Framework;

namespace Honey.Exchange.Tests
{
    public class ExchangeRateTests
    {
        static readonly Currency EUR = new Currency("EUR");
        static readonly Currency USD = new Currency("USD");
        static readonly Currency GBP = new Currency("GBP");
        
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
        public void Constructor_ThrowsInvalidPriceException_WhenPriceIsLessThanZero()
        {
            var pair = new CurrencyPair(EUR, USD);
            
            var ex = Assert.Throws<InvalidPriceException>(() => 
            {
                var rate = new ExchangeRate(pair, -1m);
            });
            Assert.AreEqual("Price cannot be less than zero (-1)", ex.Message);
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
        public void GetMoneyToExchange_ReturnsMoneyInBaseCurrencyWithDividedAmount()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(pair, 1.08m);
            var exchangeResult = new Money(5.4m, USD);

            Assert.AreEqual(EUR, rate.GetMoneyToExchange(exchangeResult).Currency);
            Assert.AreEqual(5m, rate.GetMoneyToExchange(exchangeResult).Amount);
        }

        [Test]
        public void GetMoneyToExchange_ThrowsInvalidCurrencyException_WhenPassedCurrencyIsNotQuoteCurrency()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(pair, 1.08m);
            var exchangeResult = new Money(5m, EUR);

            Assert.Throws<InvalidCurrencyException>(() => rate.GetMoneyToExchange(exchangeResult));
        }

        [Test]
        public void GetMoneyToExchange_And_Exchange_AreOppositeOperations()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(pair, 1.08m);
            var moneyToSell = new Money(5m, EUR);
            
            var exchangeResult = rate.Exchange(moneyToSell);
            
            Assert.AreEqual(moneyToSell, rate.GetMoneyToExchange(exchangeResult));
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

        private static decimal[][] _multiplications = 
        {
            new [] {1m, 2m, 2m},
            new [] {3.14m, 2m, 6.28m},
            new [] {3.14m, 2.1m, 6.594m}
        };
        [TestCaseSource(nameof(_multiplications))]
        public void Multiply_ReturnsMultipliedRate(decimal price, decimal multiplier, decimal result)
        {
            var rate = new ExchangeRate(new CurrencyPair(USD, EUR), price);
            var multipliedRate = rate * multiplier;
            
            Assert.AreEqual(result, multipliedRate.Price);
        }

        [Test]
        public void Multiply_ThrowsInvalidPriceException_WhenMultiplierIsNegative()
        {
            var rate = new ExchangeRate(new CurrencyPair(USD, EUR), 10m);

            Assert.Throws<InvalidPriceException>(() =>
            {                
                var multipliedRate = rate * -1m;
            });
        }
        
        private static decimal[][] _divisions = 
        {
            new [] {1m, 2m, 0.5m},
            new [] {3.14m, 2m, 1.57m}
        };
        [TestCaseSource(nameof(_divisions))]
        public void Division_ReturnsDividedRate(decimal price, decimal divisor, decimal result)
        {
            var rate = new ExchangeRate(new CurrencyPair(USD, EUR), price);
            var multipliedRate = rate / divisor;
            
            Assert.AreEqual(result, multipliedRate.Price);
        }

        [Test]
        public void Division_ThrowsDivideByZeroException_WhenDivisorIsZero()
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var result = new ExchangeRate(new CurrencyPair(USD, EUR), 10m) / 0;
            });
        }

        [Test]
        public void Division_ThrowsDivideByZeroException_WhenDivisorIsNegative()
        {
            Assert.Throws<InvalidPriceException>(() =>
            {
                var result = new ExchangeRate(new CurrencyPair(USD, EUR), 10m) / -1m;
            });
        }

        private static TestCaseData[] _parseValid = new[]
        {
            new ExchangeRate(new CurrencyPair(EUR, USD), 1.08m),
            new ExchangeRate(new CurrencyPair(EUR, USD), 1m),
            new ExchangeRate(new CurrencyPair(EUR, GBP), 0.8m)
        }.Select(x => new TestCaseData(x, x.ToString())).ToArray();
        [TestCaseSource(nameof(_parseValid))]
        public void Parse_ReturnsExchangeRateFromValidString(ExchangeRate rate, string s)
        {
            Assert.AreEqual(rate, ExchangeRate.Parse(s));
        }

        private static string[] _parseInvalid =
        {
            "",
            "USD/GBP",
            "USD/GBP 1.08",
            "USD/GBP rate: ",
            "USD rate: 1",
            "123"
        };
        [TestCaseSource(nameof(_parseInvalid))]
        public void Parse_ThrowsFormatException_WhenStringIsNotValid(string s)
        {
            Assert.Throws<FormatException>(() => ExchangeRate.Parse(s));
        }

        [Test]
        public void Parse_ThrowsArgumentNullException_WhenStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ExchangeRate.Parse(null));
        }
    }
}
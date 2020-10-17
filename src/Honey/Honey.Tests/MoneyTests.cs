using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace Honey.Tests
{
    public class MoneyTests
    {
        private static decimal[] _amounts = { -1m, 0m, 2m, 3.14m };
        private static string[] _currencies = { "USD", "EUR", "GBP" };
        
        [TestCaseSource(nameof(_amounts))]
        public void Amount_IsTheSameAsPassedInConstructor(decimal amount)
        {
            var euros = new Money(amount, new Currency("EUR"));
            Assert.AreEqual(amount, euros.Amount);
        }
        
        [TestCaseSource(nameof(_currencies))]
        public void Currency_IsTheSameAsPassedInConstructor(string currencyCode)
        {
            var money = new Money(10, new Currency(currencyCode));
            Assert.AreEqual(currencyCode, money.Currency.ToString());
        }
        
        [Test]
        public void Equals_ReturnsTrue_WhenCurrencyAndAmountAreTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsTrue(money1 == money2);
            Assert.IsTrue(money1.Equals(money2));
            Assert.IsTrue(money2.Equals(money1));
            Assert.AreEqual(money1, money2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyIsTheSameButAmountIsDifferent()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("USD"));
            
            Assert.IsTrue(money1 != money2);
            Assert.IsFalse(money1.Equals(money2));
            Assert.IsFalse(money2.Equals(money1));
            Assert.AreNotEqual(money1, money2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyIsDifferentButAmountIsTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("EUR"));
            
            Assert.IsTrue(money1 != money2);
            Assert.IsFalse(money1.Equals(money2));
            Assert.IsFalse(money2.Equals(money1));
            Assert.AreNotEqual(money1, money2);
        }
        
        [Test]
        public void Equals_ReturnsFalse_WhenCurrencyAndAmountAreDifferent()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("EUR"));
            
            Assert.IsTrue(money1 != money2);
            Assert.IsFalse(money1.Equals(money2));
            Assert.IsFalse(money2.Equals(money1));
            Assert.AreNotEqual(money1, money2);
        }

        [Test]
        public void Compare_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("EUR"));
            
            Assert.Throws<InvalidCurrencyException>(() => money1.CompareTo(money2));
        }
        
        [Test]
        public void Compare_ReturnsMinusOne_WhenAmountIsLess()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("USD"));
            
            Assert.AreEqual(-1, money1.CompareTo(money2));
        }
        
        [Test]
        public void Compare_ReturnsZero_WhenAmountIsTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.AreEqual(0, money1.CompareTo(money2));
        }
        
        [Test]
        public void Compare_ReturnsPlusOne_WhenAmountIsGreater()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.AreEqual(1, money1.CompareTo(money2));
        }
        
        [Test]
        public void GreaterThan_ReturnsTrue_WhenAmountIsGreater()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsTrue(money1 > money2);
        }
        
        [Test]
        public void GreaterThan_ReturnsFalse_WhenAmountIsTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsFalse(money1 > money2);
        }
        
        [Test]
        public void GreaterThan_ReturnsFalse_WhenAmountIsLess()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("USD"));
            
            Assert.IsFalse(money1 > money2);
        }
        
        [Test]
        public void LessThan_ReturnsTrue_WhenAmountIsLess()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("USD"));
            
            Assert.IsTrue(money1 < money2);
        }
        
        [Test]
        public void LessThan_ReturnsFalse_WhenAmountIsTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsFalse(money1 < money2);
        }
        
        [Test]
        public void LessThan_ReturnsFalse_WhenAmountIsGreater()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsFalse(money1 < money2);
        }
        
        [Test]
        public void GreaterThan_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("EUR"));

            Assert.Throws<InvalidCurrencyException>(() =>
            {
                var result = money1 > money2;
            });
        }
        
        [Test]
        public void LessThan_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("EUR"));
            
            Assert.Throws<InvalidCurrencyException>(() =>
            {
                var result = money1 < money2;
            });
        }
        
        [Test]
        public void GreaterOrEqualThan_ReturnsTrue_WhenAmountIsGreater()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsTrue(money1 >= money2);
        }
        
        [Test]
        public void GreaterOrEqualThan_ReturnsTrue_WhenAmountIsTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsTrue(money1 >= money2);
        }
        
        [Test]
        public void GreaterOrEqualThan_ReturnsFalse_WhenAmountIsLess()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("USD"));
            
            Assert.IsFalse(money1 >= money2);
        }
        
        [Test]
        public void LessOrEqualThan_ReturnsTrue_WhenAmountIsLess()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("USD"));
            
            Assert.IsTrue(money1 <= money2);
        }
        
        [Test]
        public void LessOrEqualThan_ReturnsTrue_WhenAmountIsTheSame()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsTrue(money1 <= money2);
        }
        
        [Test]
        public void LessOrEqualThan_ReturnsFalse_WhenAmountIsGreater()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("USD"));
            
            Assert.IsFalse(money1 <= money2);
        }
        
        [Test]
        public void GreaterOrEqualThan_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("EUR"));

            Assert.Throws<InvalidCurrencyException>(() =>
            {
                var result = money1 >= money2;
            });
        }
        
        [Test]
        public void LessOrEqualThan_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("EUR"));
            
            Assert.Throws<InvalidCurrencyException>(() =>
            {
                var result = money1 <= money2;
            });
        }

        private static decimal[][] _additions = 
        {
            new [] {1m, 2m, 3m},
            new [] {1m, 0m, 1m},
            new [] {-1m, -2m, -3m},
            new [] {-1m, 1m, 0m}
        };
        [TestCaseSource(nameof(_additions))]
        public void Addition_ReturnsSumOfAmounts(decimal amount1, decimal amount2, decimal expectedResult)
        {
            var money1 = new Money(amount1, new Currency("USD"));
            var money2 = new Money(amount2, new Currency("USD"));
            
            Assert.AreEqual(expectedResult, (money1 + money2).Amount);
            Assert.AreEqual(money1.Currency, (money1 + money2).Currency);
        }
        
        private static decimal[][] _subtractions = 
        {
            new [] {1m, 2m, -1m},
            new [] {1m, 0m, 1m},
            new [] {-1m, -2m, 1m},
            new [] {-1m, 1m, -2m}
        };
        [TestCaseSource(nameof(_subtractions))]
        public void Subtraction_ReturnsDifferenceOfAmounts(decimal amount1, decimal amount2, decimal expectedResult)
        {
            var money1 = new Money(amount1, new Currency("USD"));
            var money2 = new Money(amount2, new Currency("USD"));
            
            Assert.AreEqual(expectedResult, (money1 - money2).Amount);
            Assert.AreEqual(money1.Currency, (money1 - money2).Currency);
        }
        
        [Test]
        public void Addition_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(11m, new Currency("USD"));
            var money2 = new Money(10m, new Currency("EUR"));

            Assert.Throws<InvalidCurrencyException>(() =>
            {
                var result = money1 + money2;
            });
        }
        
        [Test]
        public void Subtraction_ThrowsException_WhenCurrenciesMismatched()
        {
            var money1 = new Money(10m, new Currency("USD"));
            var money2 = new Money(11m, new Currency("EUR"));
            
            Assert.Throws<InvalidCurrencyException>(() =>
            {
                var result = money1 - money2;
            });
        }

        [TestCaseSource(nameof(_amounts))]
        public void UnaryPlus_ReturnsTheSameMoney(decimal amount)
        {
            var money = new Money(amount, new Currency("USD"));
            
            Assert.AreEqual(money, +money);
        }
        
        [TestCaseSource(nameof(_amounts))]
        public void UnaryMinus_ReturnsMoneyWithNegatedAmount(decimal amount)
        {
            var money = new Money(amount, new Currency("USD"));
            var expected = new Money(-amount, new Currency("USD"));
            
            Assert.AreEqual(expected, -money);
        }
        
        private static decimal[][] _multiplications = 
        {
            new [] {1m, 2m, 2m},
            new [] {1m, 0m, 0m},
            new [] {-1m, -2m, 2m},
            new [] {-1m, 2m, -2m},
            new [] {3.14m, 2m, 6.28m},
            new [] {3.14m, 2.1m, 6.594m}
        };
        [TestCaseSource(nameof(_multiplications))]
        public void Multiply_ReturnsMultipliedMoney(decimal amount, decimal multiplier, decimal result)
        {
            var money = new Money(amount, new Currency("USD"));
            var multipliedMoney = money * multiplier;
            
            Assert.AreEqual(result, multipliedMoney.Amount);
        }
        
        private static decimal[][] _divisions = 
        {
            new [] {1m, 2m, 0.5m},
            new [] {0m, 3m, 0m},
            new [] {-1m, -2m, 0.5m},
            new [] {-1m, 2m, -0.5m},
            new [] {3.14m, 2m, 1.57m}
        };
        [TestCaseSource(nameof(_divisions))]
        public void Division_ReturnsDividedMoney(decimal amount, decimal divisor, decimal result)
        {
            var money = new Money(amount, new Currency("USD"));
            var dividedMoney = money / divisor;
            
            Assert.AreEqual(result, dividedMoney.Amount);
        }

        [Test]
        public void Division_ThrowsException_WhenDivisorIsZero()
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var result = new Money(10m, new Currency("USD")) / 0;
            });
        }

        private static TestCaseData[] _toString =
        {
            new TestCaseData(5m, "USD", "USD 5"),
            new TestCaseData(5.0m, "USD", "USD 5.0"),
            new TestCaseData(3.14m, "USD", "USD 3.14"),
            new TestCaseData(-3.14m, "USD", "USD -3.14"),
            new TestCaseData(-0m, "USD", "USD 0"),
            new TestCaseData(0m, "USD", "USD 0"),
            new TestCaseData(10m, "$", "$ 10")
        };
        [TestCaseSource(nameof(_toString))]
        public void ToString_Returns_CurrencyFirstThenAmount(decimal amount, string currency, string result)
        {
            var money = new Money(amount, new Currency(currency));
            
            Assert.AreEqual(result, money.ToString());
        }

        private static TestCaseData[] _roundUp =
        {
            new TestCaseData(1m, 2, "1.00"),
            new TestCaseData(1.0m, 2, "1.00"),
            new TestCaseData(1.00m, 2, "1.00"),
            new TestCaseData(1.000m, 2, "1.00"),
            new TestCaseData(1.001m, 2, "1.01"),
            new TestCaseData(1.009m, 2, "1.01"),
            new TestCaseData(1.01m, 2, "1.01"),
            new TestCaseData(1.09m, 2, "1.09"),
            new TestCaseData(1.09m, 0, "2"),
            new TestCaseData(1m, 28, "1.0000000000000000000000000000")
        };
        [TestCaseSource(nameof(_roundUp))]
        public void RoundUp_ReturnsNewMoneyWithRoundedUpAmount(decimal amount, int precision, string result)
        {
            var money = new Money(amount, new Currency("USD"));
            
            Assert.AreEqual(result, money.RoundUp(precision).Amount.ToString(CultureInfo.InvariantCulture));
        }
        
        [TestCaseSource(nameof(_roundUp))]
        public void RoundUpWithPrecisionProvider_ReturnsNewMoneyWithRoundedUpAmount(decimal amount, int precision, string result)
        {
            var money = new Money(amount, new Currency("USD"));
            var provider = new PrecisionProvider(currency => precision);
            
            Assert.AreEqual(result, money.RoundUp(provider).Amount.ToString(CultureInfo.InvariantCulture));
        }
        
        private static TestCaseData[] _roundDown =
        {
            new TestCaseData(1m, 2, "1.00"),
            new TestCaseData(1.0m, 2, "1.00"),
            new TestCaseData(1.00m, 2, "1.00"),
            new TestCaseData(1.000m, 2, "1.00"),
            new TestCaseData(1.001m, 2, "1.00"),
            new TestCaseData(1.009m, 2, "1.00"),
            new TestCaseData(1.01m, 2, "1.01"),
            new TestCaseData(1.09m, 2, "1.09"),
            new TestCaseData(1.09m, 0, "1"),
            new TestCaseData(1m, 28, "1.0000000000000000000000000000")
        };
        [TestCaseSource(nameof(_roundDown))]
        public void RoundDown_ReturnsNewMoneyWithRoundedDownAmount(decimal amount, int precision, string result)
        {
            var money = new Money(amount, new Currency("USD"));
            
            Assert.AreEqual(result, money.RoundDown(precision).Amount.ToString(CultureInfo.InvariantCulture));
        }
        
        [TestCaseSource(nameof(_roundDown))]
        public void RoundDownWithPrecisionProvider_ReturnsNewMoneyWithRoundedDownAmount(decimal amount, int precision, string result)
        {
            var money = new Money(amount, new Currency("USD"));
            var provider = new PrecisionProvider(currency => precision);
            
            Assert.AreEqual(result, money.RoundDown(provider).Amount.ToString(CultureInfo.InvariantCulture));
        }

        private static TestCaseData[] _parseValid = _amounts
            .SelectMany(a => _currencies.Select(c => new Money(a, new Currency(c))))
            .Select(x => new TestCaseData(x, x.ToString()))
            .ToArray();
        [TestCaseSource(nameof(_parseValid))]
        public void Parse_ReturnsMoneyFromValidString(Money money, string s)
        {
            Assert.AreEqual(money, Money.Parse(s));
        }

        private static string[] _parseInvalid =
        {
            "USD0",
            "1 USD",
            "USD",
            "USD ",
            "123"
        };
        [TestCaseSource(nameof(_parseInvalid))]
        public void Parse_ThrowsFormatException_WhenStringIsNotValid(string s)
        {
            Assert.Throws<FormatException>(() => Money.Parse(s));
        }

        [Test]
        public void Parse_ThrowsArgumentNullException_WhenStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Money.Parse(null));
        }
    }
}
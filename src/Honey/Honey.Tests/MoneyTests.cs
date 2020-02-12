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
    }
}
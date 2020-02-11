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
    }
}
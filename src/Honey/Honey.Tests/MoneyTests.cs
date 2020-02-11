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
    }
}
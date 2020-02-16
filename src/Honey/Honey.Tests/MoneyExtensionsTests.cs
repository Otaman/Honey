using NUnit.Framework;

namespace Honey.Tests
{
    public class MoneyExtensionsTests
    {
        private Currency USD = new Currency("USD");
        
        [Test]
        public void InExtension_CreatesMoney()
        {
            var money1 = new Money(1m, USD);
            var money2 = 1m.In(USD);
            
            Assert.AreEqual(money1, money2);
        }
    }
}
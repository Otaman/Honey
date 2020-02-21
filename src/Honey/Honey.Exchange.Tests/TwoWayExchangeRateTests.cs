using NUnit.Framework;

namespace Honey.Exchange.Tests
{
    public class TwoWayExchangeRateTests
    {
        readonly Currency EUR = new Currency("EUR");
        readonly Currency USD = new Currency("USD");
        readonly Currency GBP = new Currency("GBP");

        [Test]
        public void Constructor_CreatesRateWithCorrectProperties()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 1.1m, 0.9m);

            Assert.AreEqual(pair, rate.Pair);
            Assert.AreEqual(1.1m, rate.Bid);
            Assert.AreEqual(0.9m, rate.Ask);
        }

        [Test]
        public void Constructor_ThrowsInvalidPriceException_WhenBidOrAskIsZero()
        {
            var pair = new CurrencyPair(EUR, USD);

            Assert.Throws<InvalidPriceException>(() => new TwoWayExchangeRate(pair, 0m, 0.9m));
            Assert.Throws<InvalidPriceException>(() => new TwoWayExchangeRate(pair, 1.1m, 0m));
        }

        [Test]
        public void Constructor_ThrowsInvalidPriceException_WhenBidOrAskIsLessThanZero()
        {
            var pair = new CurrencyPair(EUR, USD);

            Assert.Throws<InvalidPriceException>(() => new TwoWayExchangeRate(pair, -1m, 0.9m));
            Assert.Throws<InvalidPriceException>(() => new TwoWayExchangeRate(pair, 1.1m, -1m));
        }
    }
}
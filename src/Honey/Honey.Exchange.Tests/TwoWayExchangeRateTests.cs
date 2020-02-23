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

        [Test]
        public void GetDirectExchangeRate_ReturnsExchangeRateWithBidPrice()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);

            var directRate = rate.GetDirectExchangeRate();

            Assert.AreEqual(pair, directRate.Pair);
            Assert.AreEqual(0.9m, directRate.Price);
        }

        [Test]
        public void GetRevercedExchangeRate_ReturnsExchangeRateWithBidPrice()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);

            var revercedRate = rate.GetRevercedExchangeRate();

            Assert.AreEqual(new CurrencyPair(USD, EUR), revercedRate.Pair);
            Assert.AreEqual(1 / 1.1m, revercedRate.Price);
        }

        [Test]
        public void Exchange_ReturnsExchangedMoneyUsingBidPrice_WhenCurrencyEqualsBase()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);
            var moneyToSell = new Money(10m, EUR);

            var boughtMoney = rate.Exchange(moneyToSell);

            Assert.AreEqual(USD, boughtMoney.Currency);
            Assert.AreEqual(9m, boughtMoney.Amount);
        }

        [Test]
        public void Exchange_ReturnsExchangedMoneyUsingOneDividedByAskPrice_WhenCurrencyEqualsQuote()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);
            var moneyToSell = new Money(10m, USD);

            var boughtMoney = rate.Exchange(moneyToSell);

            Assert.AreEqual(EUR, boughtMoney.Currency);
            Assert.AreEqual(10m/1.1m, boughtMoney.Amount);
        }

        [Test]
        public void Exchange_ThrowsInvalidCurrencyException_WhenCurrencyNotEqualsToBaseOrQuote()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);
            var moneyToSell = new Money(10m, GBP);

            Assert.Throws<InvalidCurrencyException>(() => rate.Exchange(moneyToSell));
        }

        [Test]
        public void Invert_CreatesInvertedTwoWayExchangeRate()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);

            var inverted = rate.Invert();

            Assert.AreEqual(new CurrencyPair(USD, EUR), inverted.Pair);
            Assert.AreEqual(1m/1.1m, inverted.Bid);
            Assert.AreEqual(1m/0.9m, inverted.Ask);
        }

        [Test]
        public void ToString_ReturnsPairFirstThenBidSlashAsk()
        {
            var pair = new CurrencyPair(EUR, USD);
            var rate = new TwoWayExchangeRate(pair, 0.9m, 1.1m);

            Assert.AreEqual("EUR/USD rate: 0.9/1.1", rate.ToString());
        }
    }
}
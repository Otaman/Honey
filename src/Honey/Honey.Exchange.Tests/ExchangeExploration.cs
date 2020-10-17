namespace Honey.Exchange.Tests
{
    public class ExchangeExploration
    {
        public void Explore()
        {
            var USD = new Currency("USD");
            var EUR = new Currency("EUR");

            var EurUsd = new CurrencyPair(EUR, USD);
            var rate = new ExchangeRate(EurUsd, 1.1m); // EUR/USD rate: 1.1

            var euros = 50m.In(EUR);
            var dollars = rate.Exchange(euros); // 55 USD            
        }
    }
}
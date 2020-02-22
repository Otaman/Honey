namespace Honey.Exchange
{
    public struct TwoWayExchangeRate
    {
        public CurrencyPair Pair { get; }
        public decimal Bid { get; }
        public decimal Ask { get; }

        public TwoWayExchangeRate(CurrencyPair pair, decimal bid, decimal ask)
        {
            if(bid == 0 || ask == 0) throw InvalidPriceException.PriceCannotBeZero;
            if(bid < 0 || ask < 0) throw InvalidPriceException.PriceCannotBeLessThanZero(bid);

            this.Pair = pair;
            this.Bid = bid;
            this.Ask = ask;
        }

        public ExchangeRate GetDirectExchangeRate() => 
            new ExchangeRate(Pair, Bid);

        public ExchangeRate GetRevercedExchangeRate() =>
            new ExchangeRate(new CurrencyPair(Pair.QuoteCurrency, Pair.BaseCurrency), 1m / Ask);
        
        public Money Exchange(Money moneyToSell)
        {
            if(Pair.BaseCurrency == moneyToSell.Currency)
                return GetDirectExchangeRate().Exchange(moneyToSell);

            if(Pair.QuoteCurrency == moneyToSell.Currency)
                return GetRevercedExchangeRate().Exchange(moneyToSell);

            throw new InvalidCurrencyException(Pair.BaseCurrency, moneyToSell.Currency);
        }

        public TwoWayExchangeRate Invert() => 
            new TwoWayExchangeRate(Pair.Swap(), 1m / Ask, 1m / Bid);
    }
}
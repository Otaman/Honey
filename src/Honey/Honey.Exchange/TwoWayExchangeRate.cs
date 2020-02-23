using System;

namespace Honey.Exchange
{
    public struct TwoWayExchangeRate : IEquatable<TwoWayExchangeRate>
    {
        public CurrencyPair Pair { get; }
        public decimal Bid { get; }
        public decimal Ask { get; }

        public TwoWayExchangeRate(CurrencyPair pair, decimal bid, decimal ask)
        {
            if(bid == 0 || ask == 0) throw InvalidPriceException.PriceCannotBeZero;
            if(bid < 0 || ask < 0) throw InvalidPriceException.PriceCannotBeLessThanZero(bid);

            Pair = pair;
            Bid = bid;
            Ask = ask;
        }

        public ExchangeRate GetDirectExchangeRate() => 
            new ExchangeRate(Pair, Bid);

        public ExchangeRate GetInvertedExchangeRate() =>
            new ExchangeRate(new CurrencyPair(Pair.QuoteCurrency, Pair.BaseCurrency), 1m / Ask);
        
        public Money Exchange(Money moneyToSell)
        {
            if(Pair.BaseCurrency == moneyToSell.Currency)
                return GetDirectExchangeRate().Exchange(moneyToSell);

            if(Pair.QuoteCurrency == moneyToSell.Currency)
                return GetInvertedExchangeRate().Exchange(moneyToSell);

            throw new InvalidCurrencyException(Pair.BaseCurrency, moneyToSell.Currency);
        }

        public TwoWayExchangeRate Invert() => 
            new TwoWayExchangeRate(Pair.Swap(), 1m / Ask, 1m / Bid);

        public override string ToString() => 
            $"{Pair} rate: {Bid}/{Ask}";

        public bool Equals(TwoWayExchangeRate other) => 
            Pair.Equals(other.Pair) && Bid == other.Bid && Ask == other.Ask;

        public override bool Equals(object obj) => 
            obj is TwoWayExchangeRate other && Equals(other);

        public override int GetHashCode() => 
            HashCode.Combine(Pair, Bid, Ask);

        public static bool operator ==(TwoWayExchangeRate r1, TwoWayExchangeRate r2) =>
            r1.Equals(r2);
        
        public static bool operator !=(TwoWayExchangeRate r1, TwoWayExchangeRate r2) =>
            !r1.Equals(r2);
    }
}
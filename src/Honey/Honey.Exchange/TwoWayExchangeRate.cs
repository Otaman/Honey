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
    }
}
using System;
using System.Globalization;

namespace Honey.Exchange
{
    /// <summary>
    /// Provides prices of the base currency in the quote currency for both buy and sell operations.
    /// </summary>
    /// <example>
    /// For example, the exchange rate for the EUR/USD pair is 1.07/1.09.
    /// Selling one euro you will get 1.07 US dollars (one dollar and 7 cents).
    /// Buying one euro you will spend 1.09 US dollars (one dollar and 9 cents).
    /// </example>
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

        /// <summary>
        /// Returns rate with price for current pair
        /// </summary>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.07/1.09.
        /// The result will be EUR/USD rate: 1.07
        /// </example>
        public ExchangeRate GetDirectExchangeRate() => 
            new ExchangeRate(Pair, Bid);

        /// <summary>
        /// Returns rate with price for opposite pair
        /// </summary>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.07/1.09.
        /// The result will be USD/EUR rate: 0.92
        /// </example>
        public ExchangeRate GetInvertedExchangeRate() =>
            new ExchangeRate(Pair.Swap(), 1m / Ask);
        
        /// <summary>
        /// <para>
        /// Exchange operation depends on money to sell currency.
        /// </para>
        /// <para>
        /// When money's currency is base currency then exchange means
        /// sell money in the base currency and buy money in the quote currency using bid price.
        /// </para>
        /// <para>
        /// When money's currency is quote currency then exchange means
        /// buy money in the base currency and sell money in the quote currency using ask price.
        /// </para>
        /// </summary>
        /// <param name="moneyToSell">Money to exchange.</param>
        /// <returns>Money in the opposite currency.</returns>
        /// <exception cref="InvalidCurrencyException">
        /// The currency of money to sell must be the same as the base currency or the quote currency.
        /// </exception>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.07/1.09.
        /// Exchanging one euro you will get 1*1.07 = 1.07 US dollars.
        /// Exchanging one dollar you will get 1/1.09 = 0.92 euros.
        /// </example>
        public Money Exchange(Money moneyToSell)
        {
            var sellCurrency = moneyToSell.Currency;

            if (Pair.BaseCurrency == sellCurrency)
                return GetDirectExchangeRate().Exchange(moneyToSell);

            if(Pair.QuoteCurrency == sellCurrency)
                return GetInvertedExchangeRate().Exchange(moneyToSell);

            throw new InvalidCurrencyException(Pair.BaseCurrency, sellCurrency);
        }
        
        /// <summary>
        /// <para>
        /// Calculation depends on money to buy currency.
        /// </para>
        /// <para>
        /// When money's currency is base currency then calculation means
        /// get required money in quote currency to get provided money after exchange using ask price.
        /// </para>
        /// <para>
        /// When money's currency is quote currency then calculation means
        /// get required money in base currency to get provided money after exchange using bid price.
        /// </para>
        /// </summary>
        /// <param name="moneyToBuy">Money that will be got during an exchange</param>
        /// <returns>Required money to spend to get requested money in result of exchange</returns>
        /// <exception cref="InvalidCurrencyException">
        /// The currency of provided money must be the same as the base currency or the quote currency.
        /// </exception>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.07/1.09.
        /// To get one US dollar you need 1/1.07 = 0.93 euros
        /// To get one euro you need 1*1.09 = 1.09 US dollars
        /// </example>
        public Money GetMoneyToExchange(Money moneyToBuy)
        {
            var buyCurrency = moneyToBuy.Currency;

            if (Pair.QuoteCurrency == buyCurrency)
                return GetDirectExchangeRate().GetMoneyToExchange(moneyToBuy);

            if(Pair.BaseCurrency == buyCurrency)
                return GetInvertedExchangeRate().GetMoneyToExchange(moneyToBuy); 
            
            throw new InvalidCurrencyException(Pair.QuoteCurrency, buyCurrency);
        }

        /// <summary>
        /// Provides prices for the opposite currency pair
        /// for both buying and selling operations.
        /// </summary>
        public TwoWayExchangeRate Invert() => 
            new TwoWayExchangeRate(Pair.Swap(), 1m / Ask, 1m / Bid);

        /// <summary>
        /// Returns rate in format "Pair rate: Bid/Ask" in the invariant culture
        /// </summary>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.07/1.09.
        /// Then result will be "EUR/USD rate: 1.07/1.09"
        /// </example>
        public override string ToString() => 
            $"{Pair} rate: {Bid.ToString(CultureInfo.InvariantCulture)}/{Ask.ToString(CultureInfo.InvariantCulture)}";

        public bool Equals(TwoWayExchangeRate other) => 
            Pair == other.Pair && Bid == other.Bid && Ask == other.Ask;

        public override bool Equals(object obj) => 
            obj is TwoWayExchangeRate other && Equals(other);

        public override int GetHashCode() => 
            HashCode.Combine(Pair, Bid, Ask);

        public static bool operator ==(TwoWayExchangeRate r1, TwoWayExchangeRate r2) =>
            r1.Equals(r2);
        
        public static bool operator !=(TwoWayExchangeRate r1, TwoWayExchangeRate r2) =>
            !r1.Equals(r2);
        
        public static TwoWayExchangeRate Parse(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            const string delimiter = " rate: ";
            var delimiterPosition = s.IndexOf(delimiter, StringComparison.Ordinal);
            if(delimiterPosition == -1)
                throw new FormatException("Input string was not in a correct format.");
            
            var currencyPairPart = s.Substring(0, delimiterPosition);
            var pricePart = s.Substring(delimiterPosition + delimiter.Length);

            const char priceDelimiter = '/';
            var priceDelimiterPosition = pricePart.IndexOf(priceDelimiter, StringComparison.Ordinal);
            if(priceDelimiterPosition == -1)
                throw new FormatException("Input string was not in a correct format.");

            var bidPart = pricePart.Substring(0, priceDelimiterPosition);
            var askPart = pricePart.Substring(priceDelimiterPosition + 1);

            try
            {
                var currencyPair = CurrencyPair.Parse(currencyPairPart);
                var bid = decimal.Parse(bidPart, CultureInfo.InvariantCulture);
                var ask = decimal.Parse(askPart, CultureInfo.InvariantCulture);
                
                return new TwoWayExchangeRate(currencyPair, bid, ask);
            }
            catch (Exception e) when(!(e is FormatException))
            {
                throw new FormatException("Input string was not in a correct format.", e);
            }
        }
    }
}
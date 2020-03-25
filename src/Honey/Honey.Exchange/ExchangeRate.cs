using System;
using System.Globalization;

namespace Honey.Exchange
{
    /// <summary>
    /// Provides price of the base currency in the quote currency.
    /// </summary>
    /// <example>
    /// For example, the exchange rate for the EUR/USD pair is 1.08.
    /// It means that one euro costs 1.08 US dollars (one dollar and 8 cents).
    /// </example>
    public struct ExchangeRate : IEquatable<ExchangeRate>, IComparable<ExchangeRate>
    {
        public CurrencyPair Pair { get; }
        public decimal Price { get; }

        /// <exception cref="InvalidPriceException">Price cannot be negative or zero.</exception>
        public ExchangeRate(CurrencyPair pair, decimal price)
        {
            if(price == 0)
                throw InvalidPriceException.PriceCannotBeZero;
            if(price < 0)
                throw InvalidPriceException.PriceCannotBeLessThanZero(price);

            Pair = pair;
            Price = price;
        }

        /// <summary>
        /// Exchange operation means sell money in the base currency and buy money in the quote currency.
        /// </summary>
        /// <param name="moneyToSell">Money to exchange in the base currency.</param>
        /// <returns>Money in the quote currency.</returns>
        /// <exception cref="InvalidCurrencyException">
        /// The currency of money to sell must be the same as the base currency.
        /// </exception>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.08.
        /// Then exchanging 10 euros you will get 10 dollars and 8 cents.
        /// </example>
        public Money Exchange(Money moneyToSell)
        {
            CheckCurrencies(Pair.BaseCurrency, moneyToSell.Currency);

            return new Money(moneyToSell.Amount * Price, Pair.QuoteCurrency);
        }

        /// <summary>
        /// Calculates required money in base currency to get provided money in quote currency after exchange
        /// </summary>
        /// <param name="moneyToBuy">Money that will be bought during an exchange</param>
        /// <returns>Required money to sell to get requested money in result of exchange</returns>
        /// <exception cref="InvalidCurrencyException">
        /// The currency of provided money must be the same as the quote currency.
        /// </exception>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.08.
        /// Then to get 10 dollars you need to sell 9.26 euros.
        /// </example>
        public Money CalculateMoneyToSell(Money moneyToBuy)
        {
            CheckCurrencies(Pair.QuoteCurrency, moneyToBuy.Currency);
            
            return new Money(moneyToBuy.Amount / Price, Pair.BaseCurrency);
        }

        public bool Equals(ExchangeRate other) => 
            Pair.Equals(other.Pair) && Price == other.Price;

        public override bool Equals(object obj) => 
            obj is ExchangeRate other && Equals(other);

        public override int GetHashCode() => 
            HashCode.Combine(Pair, Price);

        public int CompareTo(ExchangeRate other)
        {
            CheckCurrencyPairs(Pair, other.Pair);

            return Price.CompareTo(other.Price);
        }
                
        public static bool operator ==(ExchangeRate r1, ExchangeRate r2) => 
            r1.Equals(r2);

        public static bool operator !=(ExchangeRate r1, ExchangeRate r2) => 
            !r1.Equals(r2);
        
        public static bool operator >(ExchangeRate r1, ExchangeRate r2)
        {
            CheckCurrencyPairs(r1.Pair, r2.Pair);

            return r1.Price > r2.Price;
        }
        
        public static bool operator <(ExchangeRate r1, ExchangeRate r2)
        {
            CheckCurrencyPairs(r1.Pair, r2.Pair);
            
            return r1.Price < r2.Price;
        }
        
        public static bool operator >=(ExchangeRate r1, ExchangeRate r2)
        {
            CheckCurrencyPairs(r1.Pair, r2.Pair);

            return r1.Price >= r2.Price;
        }
        
        public static bool operator <=(ExchangeRate r1, ExchangeRate r2)
        {
            CheckCurrencyPairs(r1.Pair, r2.Pair);
            
            return r1.Price <= r2.Price;
        }

        public static ExchangeRate operator *(ExchangeRate rate, decimal multiplier) => 
            new ExchangeRate(rate.Pair, rate.Price * multiplier);
        
        public static ExchangeRate operator /(ExchangeRate rate, decimal divisor) => 
            new ExchangeRate(rate.Pair, rate.Price / divisor);

        /// <summary>
        /// Returns rate in format "Pair rate: Price" in the invariant culture
        /// </summary>
        /// <example>
        /// For example, the exchange rate for the EUR/USD pair is 1.08.
        /// Then result will be "EUR/USD rate: 1.08"
        /// </example>
        public override string ToString() => 
            Pair + " rate: " + Price.ToString(CultureInfo.InvariantCulture);

        private static void CheckCurrencyPairs(CurrencyPair expected, CurrencyPair actual)
        {
            if (expected != actual)
                throw new InvalidCurrencyPairException(expected, actual);
        }

        private static void CheckCurrencies(Currency expected, Currency actual)
        {
            if (expected != actual)
                throw new InvalidCurrencyException(expected, actual);
        }
    }
}
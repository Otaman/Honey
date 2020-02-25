using System;

namespace Honey.Exchange
{
    /// <summary>
    /// Represents currency pair. The first currency in the pair is called the base currency,
    /// while the second currency is called the quote currency.
    /// The price of the base currency is always calculated in units of the quote currency.
    /// </summary>
    /// <example>
    /// For example, the exchange rate for the EUR/USD pair is 1.08.
    /// It means that one euro costs 1.08 US dollars (one dollar and 8 cents).
    /// </example>
    public struct CurrencyPair : IEquatable<CurrencyPair>
    {
        public Currency BaseCurrency { get; }
        public Currency QuoteCurrency { get; }

        public CurrencyPair(Currency baseCurrency, Currency quoteCurrency)
        {
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
        }

        /// <summary>
        /// Creates the opposite currency pair.
        /// </summary>
        public CurrencyPair Swap() =>
            new CurrencyPair(QuoteCurrency, BaseCurrency);

        public bool Equals(CurrencyPair other) => 
            BaseCurrency.Equals(other.BaseCurrency) && QuoteCurrency.Equals(other.QuoteCurrency);

        public override bool Equals(object obj) => 
            obj is CurrencyPair other && Equals(other);

        public override int GetHashCode() => 
            HashCode.Combine(BaseCurrency, QuoteCurrency);

        public static bool operator ==(CurrencyPair cp1, CurrencyPair cp2) =>
            cp1.Equals(cp2);

        public static bool operator !=(CurrencyPair cp1, CurrencyPair cp2) =>
            !cp1.Equals(cp2);

        /// <summary>
        /// Combines the base currency and the quote currency separated by a slash.
        /// </summary>
        public override string ToString() => 
            BaseCurrency + "/" + QuoteCurrency;
    }
}
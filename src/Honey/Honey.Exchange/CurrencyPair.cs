using System;

namespace Honey.Exchange
{
    public struct CurrencyPair : IEquatable<CurrencyPair>
    {
        public Currency BaseCurrency { get; }
        public Currency QuoteCurrency { get; }

        public CurrencyPair(Currency baseCurrency, Currency quoteCurrency)
        {
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
        }

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

        public override string ToString() => 
            BaseCurrency + "/" + QuoteCurrency;
    }
}
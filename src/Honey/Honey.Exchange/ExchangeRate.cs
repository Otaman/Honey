using System;

namespace Honey.Exchange
{
    public struct ExchangeRate : IEquatable<ExchangeRate>, IComparable<ExchangeRate>
    {
        public CurrencyPair Pair { get; }
        public decimal Price { get; }

        public ExchangeRate(CurrencyPair pair, decimal price)
        {
            Pair = pair;
            Price = price;
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

        public override string ToString() => 
            Pair + " rate: " + Price;

        private static void CheckCurrencyPairs(CurrencyPair expected, CurrencyPair actual)
        {
            if (expected != actual)
                throw new InvalidCurrencyPairException(expected, actual);
        }
    }
}
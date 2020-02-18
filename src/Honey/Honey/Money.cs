using System;

namespace Honey
{
    public struct Money : IEquatable<Money>, IComparable<Money>
    {
        public decimal Amount { get; }
        public Currency Currency { get; }
        
        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }
        
        public bool Equals(Money other) => 
            Currency == other.Currency && Amount == other.Amount;

        public override bool Equals(object obj) => 
            obj is Money other && Equals(other);

        public override int GetHashCode() => 
            HashCode.Combine(Amount, Currency);
        
        public static bool operator ==(Money m1, Money m2) => 
            m1.Equals(m2);

        public static bool operator !=(Money m1, Money m2) => 
            !m1.Equals(m2);

        public int CompareTo(Money other)
        {
            CheckCurrencies(Currency, other.Currency);

            return Amount.CompareTo(other.Amount);
        }

        public static bool operator >(Money m1, Money m2)
        {
            CheckCurrencies(m1.Currency, m2.Currency);

            return m1.Amount > m2.Amount;
        }
        
        public static bool operator <(Money m1, Money m2)
        {
            CheckCurrencies(m1.Currency, m2.Currency);
            
            return m1.Amount < m2.Amount;
        }
        
        public static bool operator >=(Money m1, Money m2)
        {
            CheckCurrencies(m1.Currency, m2.Currency);

            return m1.Amount >= m2.Amount;
        }
        
        public static bool operator <=(Money m1, Money m2)
        {
            CheckCurrencies(m1.Currency, m2.Currency);
            
            return m1.Amount <= m2.Amount;
        }
        
        public static Money operator +(Money m1, Money m2)
        {
            CheckCurrencies(m1.Currency, m2.Currency);
            
            return new Money(m1.Amount + m2.Amount, m1.Currency);
        }
        
        public static Money operator -(Money m1, Money m2)
        {
            CheckCurrencies(m1.Currency, m2.Currency);
            
            return new Money(m1.Amount - m2.Amount, m1.Currency);
        }
        
        public static Money operator +(Money money) => 
            money;
        
        public static Money operator -(Money money) => 
            new Money(-money.Amount, money.Currency);

        public static Money operator *(Money money, decimal multiplier) => 
            new Money(money.Amount * multiplier, money.Currency);
        
        public static Money operator /(Money money, decimal divisor) => 
            new Money(money.Amount / divisor, money.Currency);

        public override string ToString() => 
            Currency + " " + Amount;

        public Money RoundUp(int precision)
        {
            var scale =  (decimal) Math.Pow(10, precision);
            var roundedAmount  = decimal.Ceiling(Amount * scale) / scale;

            while (GetPrecision(roundedAmount) < precision) 
                roundedAmount *= 1.0m;

            return new Money(roundedAmount, Currency);
        }
        
        public Money RoundDown(int precision)
        {
            var scale =  (decimal) Math.Pow(10, precision);
            var roundedAmount  = decimal.Floor(Amount * scale) / scale;
            
            while (GetPrecision(roundedAmount) < precision) 
                roundedAmount *= 1.0m;
            
            return new Money(roundedAmount, Currency);
        }
        
        private static int GetPrecision(decimal value){
            int[] bits = decimal.GetBits(value);
            return (bits[3] >> 16) & 0x7F; 
        }
        
        private static void CheckCurrencies(Currency expected, Currency actual)
        {
            if (expected != actual)
                throw new InvalidCurrencyException(expected, actual);
        }
    }
}
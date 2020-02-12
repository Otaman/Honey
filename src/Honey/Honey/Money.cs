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
            if (Currency != other.Currency)
                throw new InvalidCurrencyException(Currency, other.Currency);

            return Amount.CompareTo(other.Amount);
        }
    }
}
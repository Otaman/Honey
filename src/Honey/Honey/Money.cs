using System;
using System.Globalization;

namespace Honey
{
    /// <summary>
    /// Represents an amount defined in a specific currency.
    /// </summary>
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

        /// <summary>
        /// Returns the currency and the amount separated with a space
        /// </summary>
        /// <example>
        /// USD 12.34
        /// </example>
        public override string ToString() => 
            Currency + " " + Amount.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns money with the smallest amount in specified precision
        /// that is greater than or equal to the current amount.
        /// </summary>
        /// <param name="precision">The number of digits to the right after the decimal point</param>
        /// <example>
        /// (USD 12.3412).RoundUp(2) = (USD 12.35)
        /// </example>
        public Money RoundUp(int precision)
        {
            var scale =  (decimal) Math.Pow(10, precision);
            var roundedAmount  = decimal.Ceiling(Amount * scale) / scale;

            while (GetPrecision(roundedAmount) < precision) 
                roundedAmount *= 1.0m;

            return new Money(roundedAmount, Currency);
        }

        /// <summary>
        /// Returns money with the smallest amount in specified precision
        /// that is greater than or equal to the current amount.
        /// </summary>
        public Money RoundUp(IPrecisionProvider provider) => 
            RoundUp(provider.GetPrecision(Currency));

        /// <summary>
        /// Returns money with current amount
        /// which decimal part greater than the specified precision was discarded.
        /// </summary>
        /// <param name="precision">The number of digits to the right after the decimal point</param>
        /// <example>
        /// (USD 12.3468).RoundDown(2) = (USD 12.34)
        /// </example>
        public Money RoundDown(int precision)
        {
            var scale =  (decimal) Math.Pow(10, precision);
            var roundedAmount  = decimal.Floor(Amount * scale) / scale;
            
            while (GetPrecision(roundedAmount) < precision) 
                roundedAmount *= 1.0m;
            
            return new Money(roundedAmount, Currency);
        }
        
        /// <summary>
        /// Returns money with current amount
        /// which decimal part greater than the specified precision was discarded.
        /// </summary>
        public Money RoundDown(IPrecisionProvider provider) => 
            RoundDown(provider.GetPrecision(Currency));

        public static Money Parse(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            
            var spacePosition = s.IndexOf(' ');
            if(spacePosition == -1)
                throw new FormatException("Input string was not in a correct format.");
            
            var currencyCode = s.Substring(0, spacePosition);
            var amount = s.Substring(spacePosition + 1);

            try
            {
                return new Money(decimal.Parse(amount, CultureInfo.InvariantCulture), new Currency(currencyCode));
            }
            catch (Exception e) when(!(e is FormatException))
            {
                throw new FormatException("Input string was not in a correct format.", e);
            }
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
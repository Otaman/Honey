using System;

namespace Honey
{
    /// <summary>
    /// Represents a specific currency unit
    /// </summary>
    /// <example>
    /// USD, $, US Dollar
    /// </example>
    public struct Currency : IEquatable<Currency>
    {
        private string Value { get; }

        /// <exception cref="ArgumentException">Currency cannot be null or whitespace</exception>
        public Currency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency cannot be null or whitespace.", nameof(currencyCode));

            Value = currencyCode;
        }

        public bool Equals(Currency other) => 
            Value == other.Value;

        public override bool Equals(object obj) => 
            obj is Currency other && Equals(other);

        public override int GetHashCode() => 
            Value.GetHashCode();
        
        public static bool operator ==(Currency c1, Currency c2) => 
            c1.Equals(c2);

        public static bool operator !=(Currency c1, Currency c2) => 
            !c1.Equals(c2);

        public override string ToString() => 
            Value;
    }
}
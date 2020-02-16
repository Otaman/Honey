using System;

namespace Honey
{
    public class InvalidCurrencyException : InvalidOperationException
    {
        public InvalidCurrencyException(Currency expected, Currency actual) 
            : base($"Invalid currency {actual} when {expected} was expected")
        {
        }
    }
}
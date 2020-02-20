using System;

namespace Honey.Exchange
{
    public class InvalidCurrencyPairException : InvalidOperationException
    {
        public InvalidCurrencyPairException(CurrencyPair expected, CurrencyPair actual) 
            : base($"Invalid currency pair {actual} when {expected} was expected")
        {
        }    
    }
}
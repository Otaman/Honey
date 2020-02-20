using System;

namespace Honey.Exchange
{
    public class InvalidPriceException : InvalidOperationException
    {
        public InvalidPriceException(decimal price) 
            : base($"Invalid price {price}")
        {
        }

        public InvalidPriceException(string message) 
            : base(message)
        {
        }

        public static InvalidPriceException PriceCannotBeZero => 
            new InvalidPriceException("Price cannot be zero");
    }
}
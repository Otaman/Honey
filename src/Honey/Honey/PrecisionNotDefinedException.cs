using System;

namespace Honey
{
    public class PrecisionNotDefinedException : InvalidOperationException
    {
        public PrecisionNotDefinedException(Currency currency) : base($"Precision not defined for {currency}")
        {
        }
    }
}
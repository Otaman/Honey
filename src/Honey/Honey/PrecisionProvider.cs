using System;

namespace Honey
{
    public delegate bool TryGetPrecision(Currency currency, out int precision);
    
    public class PrecisionProvider : IPrecisionProvider
    {
        private readonly Func<Currency, int> _getPrecision;

        public PrecisionProvider(Func<Currency, int> getPrecision)
        {
            _getPrecision = getPrecision ?? throw new ArgumentNullException(nameof(getPrecision));
        }

        public PrecisionProvider(TryGetPrecision tryGetPrecision) 
            : this(tryGetPrecision, currency => throw new PrecisionNotDefinedException(currency))
        {
        }
        
        public PrecisionProvider(TryGetPrecision tryGetPrecision, Func<Currency, int> fallback)
        {
            if (tryGetPrecision == null) throw new ArgumentNullException(nameof(tryGetPrecision));
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));
            
            _getPrecision = currency =>
            {
                if (tryGetPrecision(currency, out var precision))
                    return precision;
                return fallback(currency);
            };
        }

        /// <inheritdoc cref="IPrecisionProvider"/>
        public int GetPrecision(Currency currency) => 
            _getPrecision(currency);
    }
}
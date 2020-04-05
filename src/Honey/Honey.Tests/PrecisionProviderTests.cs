using System;
using NUnit.Framework;

namespace Honey.Tests
{
    public class PrecisionProviderTests
    {
        readonly Currency USD = new Currency("USD");
        readonly Currency JPY = new Currency("JPY");
        
        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenGetPrecisionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PrecisionProvider((Func<Currency, int>) null));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenTryGetPrecisionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PrecisionProvider((TryGetPrecision) null));
        }
        
        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenTryGetPrecisionOrFallbackIsNull()
        {
            bool TryGetPrecision(Currency currency, out int precision)
            {
                precision = 0;
                return false;
            }
            
            int Fallback(Currency currency) => 2;

            Assert.Throws<ArgumentNullException>(() => new PrecisionProvider(null, null));
            Assert.Throws<ArgumentNullException>(() => new PrecisionProvider(TryGetPrecision, null));
            Assert.Throws<ArgumentNullException>(() => new PrecisionProvider(null, Fallback));
        }

        [Test]
        public void GetPrecision_UsesPrecisionFromProvidedGetPrecisionFunction()
        {
            int GetPrecision(Currency currency) => 2;
            var provider = new PrecisionProvider(GetPrecision);
            
            Assert.AreEqual(2, provider.GetPrecision(USD));
        }

        [Test]
        public void GetPrecision_UsesPrecisionFromFallbackFunction_WhenTryGetPrecisionReturnsFalse()
        {
            bool TryGetPrecision(Currency currency, out int precision)
            {
                precision = 0;
                return false;
            }
            
            int Fallback(Currency currency) => 2;
            
            var provider = new PrecisionProvider(TryGetPrecision, Fallback);
            
            Assert.AreEqual(2, provider.GetPrecision(USD));
        }
        
        [Test]
        public void GetPrecision_ThrowsException_WhenTryGetPrecisionReturnsFalseAndFallbackNotProvided()
        {
            bool TryGetPrecision(Currency currency, out int precision)
            {
                precision = 0;
                return false;
            }
            
            var provider = new PrecisionProvider(TryGetPrecision);

            var ex = Assert.Throws<PrecisionNotDefinedException>(() => provider.GetPrecision(USD));
            Assert.AreEqual("Precision not defined for USD", ex.Message);
        }

        [Test]
        public void GetPrecision_UsesPrecisionFromProvidedTryGetPrecisionFunction_WhenItReturnsTrue()
        {
            bool TryGetPrecision(Currency currency, out int precision)
            {
                if (currency == USD)
                {
                    precision = 2;
                    return true;
                }

                if (currency == JPY)
                {
                    precision = 0;
                    return true;
                }
                
                precision = 0;
                return false;
            }
            
            var provider = new PrecisionProvider(TryGetPrecision);
            
            Assert.AreEqual(2, provider.GetPrecision(USD));
            Assert.AreEqual(0, provider.GetPrecision(JPY));
        }
    }
}
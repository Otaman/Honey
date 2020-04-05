namespace Honey
{
    /// <summary>
    /// Provides precision for currencies
    /// </summary>
    public interface IPrecisionProvider
    {
        /// <summary>
        /// Returns precision for provided currency
        /// </summary>
        int GetPrecision(Currency currency);
    }
}
namespace Honey
{
    public static class MoneyExtensions
    {
        public static Money In(this decimal amount, Currency currency) => 
            new Money(amount, currency);
    }
}
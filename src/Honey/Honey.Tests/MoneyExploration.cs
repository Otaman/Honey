namespace Honey.Tests
{
    public class MoneyExploration
    {
        public void Explore()
        {
            var USD = new Currency("USD"); //or "$", or "US Dollar" -- whichever is used in your domain
            var EUR = new Currency("EUR");

            var dollars = new Money(101.52m, USD); //USD 101.52
            var euros  = 20m.In(EUR); //EUR 20

            var coffeePrice = new Money(4m, EUR); // EUR 4
            dollars -= coffeePrice; // throws InvalidCurrencyException
            euros -= coffeePrice; // EUR 16

            var twoCups = coffeePrice * 2; // EUR 8
            var hasEnoughMoney = euros >= twoCups; // true
        }
    }
}
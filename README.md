### Honey
Provides money type and operations for .NET

### Money operations examples
```C#
var USD = new Currency("USD"); //or "$", or "US Dollar" -- whichever is used in your domain
var EUR = new Currency("EUR");

var dollars = new Money(101.52m, USD); //USD 101.52
var euros  = 20m.In(EUR); //EUR 20

var coffeePrice = new Money(4m, EUR); // EUR 4
dollars -= coffeePrice; // throws InvalidCurrencyException
euros -= coffeePrice; // EUR 16

var twoCups = coffeePrice * 2; // EUR 8
var hasEnoughMoney = euros >= twoCups; // true
```

### Exchange operations examples
```C#
var USD = new Currency("USD");
var EUR = new Currency("EUR");

var EurUsd = new CurrencyPair(EUR, USD);
var rate = new ExchangeRate(EurUsd, 1.1m); // EUR/USD rate: 1.1

var euros = 50m.In(EUR);
var dollars = rate.Exchange(euros); // 55 USD  
```
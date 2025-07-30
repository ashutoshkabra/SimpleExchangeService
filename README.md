# SimpleExchangeService
Simple Exchange Service API

To run the solution
-------------------
Build the solution and run it in Visual Studio.

In Windows Command Prompt, run the following:
curl -X POST "https://localhost:7171/ExchangeService" -H "accept: */*" -H "Content-Type: application/json" -d "{\"amount\": 5, \"inputCurrency\": \"AUD\", \"outputCurrency\": \"USD\"}"

Improvements
------------
Maintainability
  • Create a separate Class library project: ForexService that is used in the API project for calculating Forex rates
  
Logging Exceptions
  • The solution requires better handling of Exceptions such as logging in log files/DB/Windows event.
  Example: The method GetExchangeRate in the ForexServices.cs doesn't handle any form of logging when trying to retrieve rates.

Testability
-----------
  • Implement the following tests in the ForexServicesTests.cs:
         * CalculateExchangeRate_ReturnsNull_OnHttpError()
         * CalculateExchangeRate_ReturnsNull_WhenRateNotFound()
         * CalculateExchangeRate_ReturnsConvertedAmount_WhenRateIsAvailable()
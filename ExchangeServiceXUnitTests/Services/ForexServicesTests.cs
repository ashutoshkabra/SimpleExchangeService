#region Using Namespaces

using FluentAssertions;
using ExchangeServiceAPI.Dtos;
using ExchangeServiceAPI.Models;
using ExchangeServiceAPI.Services;
using Microsoft.Extensions.Options;

#endregion

namespace ExchangeService_XUnitTests.Services
{
    public class ForexServicesTests
    {
        /* Immplement following test methods:
         * CalculateExchangeRate_ReturnsNull_OnHttpError()
         * CalculateExchangeRate_ReturnsNull_WhenRateNotFound()
         * CalculateExchangeRate_ReturnsConvertedAmount_WhenRateIsAvailable()
        */

        #region Internal Members

        private IForexServices _forexServices;

        #endregion

        #region Constructors

        public ForexServicesTests()
        {
            var httpClient = new HttpClient();

            var apiSettings = Options.Create(new ApiSettings
            {
                ExchangeRateUri = "https://fakeapi.com/latest/"
            });

            _forexServices = new ForexServices(apiSettings, httpClient);
        }

        #endregion

        #region Test Methods

        [Fact]
        public void ValidateExchangeRequest_ShouldReturnEmptyString_ForValidInput()
        {
            // Arrange
            var exRequest = new ExchangeRequest
            {
                InputCurrency = "AUD",
                OutputCurrency = "USD",
                Amount = 5
            };

            // Act
            var result = _forexServices.ValidateExchangeRequest(exRequest);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void ValidateExchangeRequest_ShouldReturnErrorMessages_ForInvalidCurrencyCodes()
        {
            // Arrange
            var exRequest = new ExchangeRequest
            {
                InputCurrency = "AUSA",
                OutputCurrency = "USDEUR",
                Amount = 5
            };

            // Act
            var result = _forexServices.ValidateExchangeRequest(exRequest);

            // Assert
            result.Should().Contain("InputCurrency should be 3 chars");
            result.Should().Contain("OutputCurrency should be 3 chars");
        }

        #endregion
    }
}
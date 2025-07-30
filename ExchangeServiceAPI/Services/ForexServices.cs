#region Using Namespaces

using System.Text;
using System.Text.Json;

using ExchangeServiceAPI.Dtos;
using ExchangeServiceAPI.Models;
using Microsoft.Extensions.Options;

#endregion

namespace ExchangeServiceAPI.Services
{
    public interface IForexServices
    {
        string ValidateExchangeRequest(ExchangeRequest request);
        Task<decimal?> CalculateExchangeRate(string inputCurrencyCode, string outputCurrencyCode, decimal amount);
    }

    public class ForexServices : IForexServices
    {
        #region Internal Members

        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        #endregion

        #region Constructors

        public ForexServices(IOptions<ApiSettings> apiSettings, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        #endregion

        #region Public Methods

        public async Task<decimal?> CalculateExchangeRate(string inputCurrencyCode, string outputCurrencyCode, decimal amount)
        {
            decimal? currentExchangeRate = await GetExchangeRate(inputCurrencyCode, outputCurrencyCode);

            if (currentExchangeRate != null)
            {
                return amount * currentExchangeRate.Value;
            }
            else
            {
                return null;
            }
        }

        public string ValidateExchangeRequest(ExchangeRequest request)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (request.InputCurrency.Length != 3)
            {
                stringBuilder.Append(" InputCurrency invalid. InputCurrency should be 3 chars.");
            }

            if (request.OutputCurrency.Length != 3)
            {
                stringBuilder.Append(" OutputCurrency invalid. OutputCurrency should be 3 chars.");
            }

            return stringBuilder.ToString().Trim();
        }

        #endregion

        #region Internal Methods

        private async Task<decimal?> GetExchangeRate(string inputCurrencyCode, string outputCurrencyCode)
        {
            try
            {
                string apiUri = _apiSettings.ExchangeRateUri;

                // e.g. "https://open.er-api.com/v6/latest/" + "AUD"
                HttpResponseMessage response = await _httpClient.GetAsync($"{apiUri}{inputCurrencyCode}");

                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                ForexResponse? forexData = JsonSerializer.Deserialize<ForexResponse>(jsonString, jsonSerializerOptions);

                if (forexData != null)
                {
                    decimal forexRates = forexData.Rates.FirstOrDefault(x => x.Key.Equals(outputCurrencyCode, StringComparison.InvariantCultureIgnoreCase)).Value;
                    return forexRates;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log error and return null
                return null;
            }
        }

        #endregion
    }
}
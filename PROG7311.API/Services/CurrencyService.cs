using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PROG7311.API.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _http;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(HttpClient http, ILogger<CurrencyService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            try
            {
                var resp = await _http.GetFromJsonAsync<ExchangeRateResponse>("https://open.er-api.com/v6/latest/USD");
                if (resp != null && resp.rates != null && resp.rates.TryGetValue("ZAR", out var rate))
                {
                    return Convert.ToDecimal(rate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch exchange rate");
            }

            return 18.50m;
        }
    }

    public class ExchangeRateResponse
    {
        public System.Collections.Generic.Dictionary<string, double> rates { get; set; } = new();
    }
}

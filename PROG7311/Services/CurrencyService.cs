namespace PROG7311.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(HttpClient httpClient, ILogger<CurrencyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(
                    "https://open.er-api.com/v6/latest/USD");

                if (response?.rates != null && response.rates.ContainsKey("ZAR"))
                    return (decimal)response.rates["ZAR"];
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
        public Dictionary<string, double> rates { get; set; } = new();
    }
}
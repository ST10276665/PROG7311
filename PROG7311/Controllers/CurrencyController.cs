using Microsoft.AspNetCore.Mvc;
using PROG7311.Services;

namespace PROG7311.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("usd-to-zar")]
        public async Task<IActionResult> GetUsdToZarRate()
        {
            var rate = await _currencyService.GetUsdToZarRateAsync();
            return Ok(new { rate });
        }
    }
}